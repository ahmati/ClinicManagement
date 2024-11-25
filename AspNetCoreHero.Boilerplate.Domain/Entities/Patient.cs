using AspNetCoreHero.Abstractions.Domain;
using AspNetCoreHero.Boilerplate.Domain.Common;
using System.Collections;
using System.Collections.Generic;

namespace AspNetCoreHero.Boilerplate.Domain.Entities
{
    public class Patient : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] ProfilePicture { get; set; }
         
        public ICollection<PatientTreatment> PatientTreatments{ get; set; }
    }
}
