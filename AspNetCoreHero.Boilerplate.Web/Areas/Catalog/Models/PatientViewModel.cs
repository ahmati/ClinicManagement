using AspNetCoreHero.Boilerplate.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreHero.Boilerplate.Web.Areas.Catalog.Models
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public DateTime BornDate { get; set; }
        public string Gender { get; set; }
        public string ParentName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Profession { get; set; }
        public string JobAddress { get; set; }
        public byte[] ProfilePicture { get; set; }

        public int? MedicalDataId { get; set; } 
    }
}
