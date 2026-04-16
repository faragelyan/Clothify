using AutoMapper;
using Clothify.Application.DTOs.ProductSize;
using Clothify.Domain.Entities;

namespace Clothify.Application.Mapping
{
    public class ProductSizeProfile : Profile
    {
        public ProductSizeProfile()
        {
            CreateMap<ProductSizeDto, ProductSize>().ReverseMap();
            CreateMap<CreateProductSizeDto, ProductSize>().ReverseMap();
        }
    }
}
