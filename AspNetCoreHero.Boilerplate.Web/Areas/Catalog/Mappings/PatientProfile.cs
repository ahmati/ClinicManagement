using AspNetCoreHero.Boilerplate.Application.Features;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Web.Areas.Catalog.Models;
using AutoMapper;
using Domain.Entities;
using Web.Areas.Catalog.Models;

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

            CreateMap<MedicalData, MedicalDataViewModel>().ReverseMap();
            CreateMap<UpdateMedicalDataCommand, MedicalDataViewModel>().ReverseMap();
            CreateMap<UpdateMedicalDataCommand, MedicalData>().ReverseMap();
        }
    }
}
