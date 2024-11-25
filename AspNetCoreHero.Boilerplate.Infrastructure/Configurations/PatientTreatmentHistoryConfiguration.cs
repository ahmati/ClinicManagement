using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCoreHero.Boilerplate.Infrastructure.Configurations
{
    internal class PatientTreatmentHistoryConfiguration : BaseEntityTypeConfiguration<PatientTreatment>
    {
        protected override void ConfigureOtherProperties(EntityTypeBuilder<PatientTreatment> builder)
        {
            builder.ToTable(nameof(Patient));
        }
    }
}
