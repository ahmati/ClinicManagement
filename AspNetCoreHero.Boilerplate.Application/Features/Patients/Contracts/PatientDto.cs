using AspNetCoreHero.Boilerplate.Domain.Entities;
using AutoMapper;
using CardoAI.Core.PFM.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Application.Features
{
    public class PatientDto : IMapper<Patient>
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] ProfilePicture { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Patient, PatientDto>();
            profile.CreateMap<PatientDto, Patient>();
        }

    }

}
