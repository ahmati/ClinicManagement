using AspNetCoreHero.Boilerplate.Web.Areas.Catalog.Models;

namespace Web.Areas.Catalog.Models
{
    public class MedicalDataViewModel
    {
        public int Id { get; set; } 
        // Specific illness/medical data
        public bool HasIllness { get; set; }
        public string? IllnessDetails { get; set; }
        public bool IsUnderTreatment { get; set; }
        public string? DoctorsName { get; set; }
        public string? MedicationsTreatment { get; set; }

        // Liste e semundjeve te mundshme 
        public bool SemundjeGjaku { get; set; }
        public bool TensionILarte { get; set; }
        public bool TensionIUlet { get; set; }
        public bool NderhyrjeKirugjikaleNeZemer { get; set; }
        public bool PropblemeZemre { get; set; }
        public bool EtheRaumatizmale { get; set; }
        public bool Glaucoma { get; set; }
        public bool Diabet { get; set; } 
        public bool AlergjiNgaLlastikuDorezave { get; set; }
        public bool AlergjiNgaMedikamentet { get; set; }
        public bool AlergjiNgaMetalet { get; set; }
        public bool SemundjeMendore { get; set; }
        public bool Epilepsi { get; set; }
        public bool PerdoruesDroge { get; set; }
        public bool AzemBronkiale { get; set; }
        public bool Turbekuloze { get; set; }
        public bool MjekimTumori { get; set; }
        public bool Shtatzen { get; set; }
        public bool AIDS { get; set; }
        public bool SemundjeMelcie { get; set; }

        public string ArsyejaEParaqitjesNeKlinike { get; set; }

        public int PatientId { get; set; }
        public PatientViewModel Patient { get; set; }
    }
}
