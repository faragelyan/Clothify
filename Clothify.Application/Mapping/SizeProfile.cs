using AutoMapper;
using Clothify.Application.DTOs.Size;
using Clothify.Domain.Entities;

namespace Clothify.Application.Mapping
{
    public class SizeProfile : Profile
    {
        public SizeProfile()
        {
            CreateMap<SizeDto, Size>().ReverseMap();
            CreateMap<CreateSizeDto, Size>().ReverseMap();
            CreateMap<UpdateSizeDto, Size>().ReverseMap();
        }
    }
}
