using AspNetCoreHero.Boilerplate.Application.Features;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Web.Areas.Catalog.Models;
using AutoMapper;

namespace Web.Areas.Catalog.Mappings
{
    internal class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<CreatePatientCommand, PatientViewModel>().ReverseMap();
            CreateMap<UpdatePatientCommand, PatientViewModel>().ReverseMap();
            CreateMap<PatientDto, PatientViewModel>().ReverseMap();
            CreateMap<Patient, PatientViewModel>().ReverseMap();
        }
    }
}
