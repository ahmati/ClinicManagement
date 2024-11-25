using AspNetCoreHero.Boilerplate.Application.Features;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AutoMapper;

namespace Application.Features
{
    internal class PatientTreatmentProfile : Profile
    {
        public PatientTreatmentProfile()
        {
            CreateMap<CreatePatientTreatmentCommand, PatientTreatment>().ReverseMap();
            CreateMap<UpdatePatientTreatmentCommand, PatientTreatment>().ReverseMap();
        }
    }
}
