using AspNetCoreHero.Boilerplate.Application.Features;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Web.Areas.Catalog.Models;
using AutoMapper;
using Web.Areas.Catalog.Models;

namespace Web.Areas.Catalog.Mappings
{
    internal class PatientTreatmentProfile : Profile
    {
        public PatientTreatmentProfile()
        {
            CreateMap<CreatePatientTreatmentCommand, PatientTreatmentViewModel>().ReverseMap();
            CreateMap<UpdatePatientTreatmentCommand, PatientTreatmentViewModel>().ReverseMap();
            CreateMap<PatientTreatment, PatientTreatmentViewModel>().ReverseMap();
        }
    }
}
