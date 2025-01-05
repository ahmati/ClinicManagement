using AspNetCoreHero.Abstractions.Domain;
using AspNetCoreHero.Boilerplate.Domain.Common;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class StaffUser : BaseAuditableEntity
    {
        public string Name { get; set; } 
        public string Surname { get; set; }
        public DateTime BornDate { get; set; }
        public string Role { get; set; }
        public string PhoneNo { get; set; }
        public ICollection<PatientTreatment> PatientTreatments{ get; set; }
    }
}
