using AspNetCoreHero.Boilerplate.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Areas.Catalog.Models
{
    public class PatientTreatmentViewModel
    {
        public int Id { get; set; } 
        public int Tooth { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public DateTime DateOfIntervention{ get; set; }
        public string Payment { get; set; }
        public int PatientId { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; } 
    }
}
