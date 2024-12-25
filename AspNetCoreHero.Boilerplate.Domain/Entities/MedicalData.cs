using AspNetCoreHero.Abstractions.Domain;
using AspNetCoreHero.Boilerplate.Domain.Common;
using AspNetCoreHero.Boilerplate.Domain.Entities;

namespace Domain.Entities
{
    public class MedicalData : BaseAuditableEntity
    {
        // Specific illness/medical data

        public bool HasIllness { get; set; } = false;
        public string IllnessDetails { get; set; } = string.Empty;
        public bool IsUnderTreatment { get; set; } = false;
        public string DoctorsName { get; set; } = string.Empty;
        public string MedicationsTreatment { get; set; } = string.Empty;

        // Liste e semundjeve te mundshme 
        public bool SemundjeGjaku { get; set; } = false;
        public bool TensionILarte { get; set; } = false;
        public bool TensionIUlet { get; set; } = false;
        public bool NderhyrjeKirugjikaleNeZemer { get; set; } = false;
        public bool ProblemeZemre { get; set; } = false;
        public bool EtheRaumatizmale { get; set; } = false;
        public bool Glaucoma { get; set; } = false;
        public bool Diabet { get; set; } = false;
        public bool AlergjiNgaLlastikuDorezave { get; set; } = false;
        public bool AlergjiNgaMedikamentet { get; set; } = false;
        public bool AlergjiNgaMetalet { get; set; } = false;
        public bool SemundjeMendore { get; set; } = false;
        public bool Epilepsi { get; set; } = false;
        public bool PerdoruesDroge { get; set; } = false;
        public bool AzemBronkiale { get; set; } = false;
        public bool Turbekuloze { get; set; } = false;
        public bool MjekimTumori { get; set; } = false;
        public bool Shtatzen { get; set; } = false;
        public bool AIDS { get; set; } = false;
        public bool SemundjeMelcie { get; set; } = false;

        public string ArsyejaEParaqitjesNeKlinike { get; set; } = string.Empty;
    }
}
