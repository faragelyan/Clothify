using AutoMapper;
using Clothify.Application.DTOs.UserPhone;
using Clothify.Domain.Entities;

namespace Clothify.Application.Mapping
{
    public class UserPhoneProfile : Profile
    {
        public UserPhoneProfile()
        {
            CreateMap<UserPhoneDto, UserPhone>().ReverseMap();
            CreateMap<CreateUserPhoneDto, UserPhone>().ReverseMap();
            CreateMap<UpdateUserPhoneDto, UserPhone>().ReverseMap();
        }
    }
}
