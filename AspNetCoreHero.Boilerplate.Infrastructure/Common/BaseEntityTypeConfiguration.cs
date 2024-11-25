using AspNetCoreHero.Abstractions.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Reflection;

namespace AspNetCoreHero.Boilerplate.Infrastructure.Common
{
    public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : AuditableEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            if (!typeof(T).Name.Contains("History"))
            {
                _ = builder.HasKey(x => x.Id);
            }

            //builder.Property(x => x.de).HasDefaultValue(false);

            Type type = typeof(AuditableEntity);

            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                PropertyInfo propertyInfo = type.GetProperty(property.Name);

                Type propertyType = propertyInfo?.PropertyType;
                if (propertyType == typeof(string))
                {
                    _ = builder.Property(property.Name).HasMaxLength(500);
                }

                if (property.Name == nameof(AuditableEntity.CreatedOn))
                {
                    _ = builder.Property(property.Name).HasDefaultValueSql("now() at time zone 'utc'");
                }

                if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                {
                    _ = builder.Property(property.Name).HasPrecision(18, 2);
                }
            }

            ConfigureOtherProperties(builder);
        }

        protected abstract void ConfigureOtherProperties(EntityTypeBuilder<T> builder);
    }
}
