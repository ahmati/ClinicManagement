using AspNetCoreHero.Boilerplate.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AspNetCoreHero.Boilerplate.Web.Areas.Catalog.Models
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] ProfilePicture { get; set; }

        public SelectList MedicalVisits { get; set; }
    }
}
