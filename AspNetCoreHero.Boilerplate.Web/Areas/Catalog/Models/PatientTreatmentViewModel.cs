using AspNetCoreHero.Boilerplate.Domain.Common;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Web.Areas.Catalog.Models
{
    public class PatientTreatmentViewModel
    {
        public int Id { get; set; } 
        public int Tooth { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public DateTime? DateOfIntervention{ get; set; }
        public string Payment { get; set; }
        public int PatientId { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserStaffName { get; set; }
        public int StaffUserId { get; set; } 
        public byte[] Picture { get; set; } 
        public SelectList StaffUsers { get; set; } 
    }
}
