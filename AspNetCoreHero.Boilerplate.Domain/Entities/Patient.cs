using AspNetCoreHero.Abstractions.Domain;
using AspNetCoreHero.Boilerplate.Domain.Common;
using Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AspNetCoreHero.Boilerplate.Domain.Entities
{
    public class Patient : BaseAuditableEntity
    {
        public string Name { get; set; }
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
        public MedicalData MedicalData{ get; set; }

        public ICollection<PatientTreatment> PatientTreatments{ get; set; }
    }
}
