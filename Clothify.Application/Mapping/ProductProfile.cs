using AutoMapper;
using Clothify.Application.DTOs.Product;
using Clothify.Domain.Entities;

namespace Clothify.Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<UpdateProductDto, Product>().ReverseMap();
            CreateMap<CreateProductDto, Product>().ReverseMap();
        }
    }
}
