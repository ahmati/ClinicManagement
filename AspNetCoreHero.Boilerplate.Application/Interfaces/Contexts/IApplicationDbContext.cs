using AspNetCoreHero.Boilerplate.Domain.Common;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts
{
    public interface IApplicationDbContext
    {
        IDbConnection Connection { get; }
        bool HasChanges { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbSet<T> EntitySet<T>() where T : class;
        Task<T> GetById<T>(int id) where T : class;
        Task<EntityEntry<TEntity>> CreateAsync<TEntity>(TEntity entity, bool commitChanges = true, CancellationToken cancellationToken = default) where TEntity : BaseAuditableEntity;
        Task<TEntity> UpdateAsync<TEntity>(TEntity entity, bool commitChanges = true, CancellationToken cancellationToken = default) where TEntity : BaseAuditableEntity;
        Task<bool> DeleteAsync<TEntity>(TEntity entity, bool commitChanges = true, CancellationToken cancellationToken = default) where TEntity : BaseAuditableEntity;

        DbSet<Product> Products { get; set; }
    }
}