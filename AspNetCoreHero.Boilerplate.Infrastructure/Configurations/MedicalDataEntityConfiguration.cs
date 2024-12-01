using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Infrastructure.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System;

namespace AspNetCoreHero.Boilerplate.Infrastructure.Configurations
{
    internal class MedicalDataEntityConfiguration : BaseEntityTypeConfiguration<MedicalData>
    {
        protected override void ConfigureOtherProperties(EntityTypeBuilder<MedicalData> builder)
        {
            builder.ToTable(nameof(MedicalData));
        }
    }
}
