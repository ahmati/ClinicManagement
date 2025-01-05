using AspNetCoreHero.Boilerplate.Application.Features;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AspNetCoreHero.Boilerplate.Web.Areas.Catalog.Models;
using AutoMapper;
using Domain.Entities;
using System.Linq;
using Web.Areas.Catalog.Models;

namespace Web.Areas.Catalog.Mappings
{
    internal class PatientTreatmentProfile : Profile
    {
        public PatientTreatmentProfile()
        {
            CreateMap<CreatePatientTreatmentCommand, PatientTreatmentViewModel>().ReverseMap();
            CreateMap<UpdatePatientTreatmentCommand, PatientTreatmentViewModel>().ReverseMap();
            CreateMap<UpdatePatientTreatmentCommand, PatientTreatment>().ReverseMap();
            CreateMap<PatientTreatmentViewModel, PatientTreatment>();
            CreateMap<PatientTreatment, PatientTreatmentViewModel>()
                .ForMember(x => x.UserStaffName, y => y.MapFrom(x => x.StaffUser.Name + " " + x.StaffUser.Surname));
            

            CreateMap<CreateStaffUserCommand, StaffUserViewModel>().ReverseMap();
            CreateMap<UpdateStaffUserCommand, StaffUserViewModel>().ReverseMap();
            CreateMap<StaffUserViewModel, StaffUser>().ReverseMap()
                .ForMember(x => x.FullName, y => y.MapFrom(x => x.Name + " " + x.Surname));
        }
    }
}
