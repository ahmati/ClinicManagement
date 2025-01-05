using AspNetCoreHero.Boilerplate.Application.Features;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AutoMapper;
using Domain.Entities;

namespace Application.Features
{
    internal class StaffUserProfile : Profile
    {
        public StaffUserProfile()
        {
            CreateMap<StaffUser, StaffUserDto>().ReverseMap();
            CreateMap<CreateStaffUserCommand, StaffUser>().ReverseMap();
            CreateMap<UpdateStaffUserCommand, StaffUser>().ReverseMap();
        }
    }
}
