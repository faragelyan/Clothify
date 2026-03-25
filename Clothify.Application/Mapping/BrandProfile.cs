using AutoMapper;
using Clothify.Application.DTOs.Brand;
using Clothify.Domain.Entities;

namespace Clothify.Application.Mapping
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<BrandDto, Brand>().ReverseMap();
            CreateMap<UpdateBrandDto, Brand>().ReverseMap();
            CreateMap<CreateBrandDto, Brand>().ReverseMap();
        }
    }
}
