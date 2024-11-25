using AspNetCoreHero.Abstractions.Domain;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Shared;
using AspNetCoreHero.Boilerplate.Domain.Common;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Domain.Entities.Catalog;
using AspNetCoreHero.EntityFrameworkCore.AuditTrail;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Infrastructure.DbContexts
{
    public class ApplicationDbContext : AuditableContext, IApplicationDbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private readonly ILogger<ApplicationDbContext> _logger;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser, ILogger<ApplicationDbContext> logger) : base(options)
        {
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
            _logger = logger;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Patient> Patient { get; set; }

        public IDbConnection Connection => Database.GetDbConnection();

        public bool HasChanges => ChangeTracker.HasChanges();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = _dateTime.NowUtc;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                        break;
                }
            }
            if (_authenticatedUser.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(_authenticatedUser.UserId);
            }
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (Database.GetPendingMigrations().Any())
                {
                    await Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public DbSet<T> EntitySet<T>() where T : class
        {
            return Set<T>();
        }
        
        public async Task<T> GetById<T>(int id) where T : class
        {
            return await Set<T>().FindAsync(id);
        }

        public async Task<EntityEntry<TEntity>> CreateAsync<TEntity>(TEntity entity, bool commitChanges = true, CancellationToken cancellationToken = default) where TEntity : BaseAuditableEntity
        {

            if (entity is null)
            {
                throw new ArgumentNullException();
            }

            if (entity is BaseAuditableEntity)
            {
                (entity as BaseAuditableEntity).CreatedOn = DateTime.UtcNow;
            }

            var createdEntity = Set<TEntity>().Add(entity);

            if (commitChanges)
            {
                await SaveChangesAsync(cancellationToken);
            }

            return createdEntity;
        }

        public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity, bool commitChanges = true, CancellationToken cancellationToken = default) where TEntity : BaseAuditableEntity
        {
            if (entity is null)
            {
                throw new ArgumentNullException();
            }

            if (entity is BaseAuditableEntity)
                (entity as BaseAuditableEntity).LastModifiedOn = DateTime.UtcNow;

            Set<TEntity>().Update(entity);

            if (commitChanges)
                await SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<bool> DeleteAsync<TEntity>(TEntity entity, bool commitChanges = true, CancellationToken cancellationToken = default) where TEntity : BaseAuditableEntity
        {
            if (entity is null)
            {
                throw new ArgumentNullException();
            }

            entity.IsDeleted = true;

            return (await UpdateAsync(entity, commitChanges, cancellationToken)).IsDeleted;
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var isDeletedProperty = entityType.ClrType.GetProperty("IsDeleted");
                if (isDeletedProperty != null)
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var propertyAccess = Expression.Property(parameter, "IsDeleted");
                    var condition = Expression.Equal(propertyAccess, Expression.Constant(false));
                    var lambda = Expression.Lambda(condition, parameter);

                    builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
                base.OnModelCreating(builder);
        }
    }
}