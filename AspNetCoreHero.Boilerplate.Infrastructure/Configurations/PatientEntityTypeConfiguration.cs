using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System;

namespace AspNetCoreHero.Boilerplate.Infrastructure.Configurations
{
    internal class PatientEntityTypeConfiguration : BaseEntityTypeConfiguration<Patient>
    {
        protected override void ConfigureOtherProperties(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable(nameof(Patient));
        }
    }
}
