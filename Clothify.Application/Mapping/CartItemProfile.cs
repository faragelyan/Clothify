using AutoMapper;
using Clothify.Application.DTOs.CartItem;
using Clothify.Domain.Entities;

namespace Clothify.Application.Mapping
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItemDto, CartItem>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.ShoppingCart, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : (decimal?)null))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product != null ? src.Product.ImageUrl : null));

            CreateMap<CreateCartItemDto, CartItem>()
                .ForMember(dest => dest.CartId, opt => opt.Ignore())
                .ForMember(dest => dest.AddedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.ShoppingCart, opt => opt.Ignore());

            CreateMap<UpdateCartItemDto, CartItem>()
                .ForMember(dest => dest.CartId, opt => opt.Ignore())
                .ForMember(dest => dest.AddedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.ShoppingCart, opt => opt.Ignore());
        }
    }
}
