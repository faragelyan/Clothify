using AutoMapper;
using Clothify.Application.DTOs.User;
using Clothify.Domain.Entities;

namespace Clothify.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<CreateUserDto, AppUser>().ReverseMap();
            CreateMap<UpdateUserDto, AppUser>().ReverseMap();
        }
    }
}
