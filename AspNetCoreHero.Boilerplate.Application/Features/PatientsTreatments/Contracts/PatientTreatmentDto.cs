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
    public class PatientTreatmentDto : IMapper<PatientTreatment>
    {
        public int Id { get; set; } 
        public int Tooth { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public DateTime DateOfIntervention { get; set; }
        public string Payment { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PatientTreatment, PatientTreatmentDto>().ReverseMap();
        }

    }

}
