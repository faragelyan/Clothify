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
            /*CreateMap<AppUser, CreateUserResponseDto>()
             .ForMember(dest => dest.userDTO, opt => opt.MapFrom(src => src)).ReverseMap();*/

            //CreateMap<UpdateDTO, AppUser>().ReverseMap();
        }
    }
}
