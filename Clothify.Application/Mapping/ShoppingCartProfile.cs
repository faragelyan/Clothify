using AutoMapper;
using Clothify.Application.DTOs.ShoppingCart;
using Clothify.Domain.Entities;

namespace Clothify.Application.Mapping
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<ShoppingCartDto, ShoppingCart>().ReverseMap();
            CreateMap<CreateShoppingCartDto, ShoppingCart>().ReverseMap();
            CreateMap<UpdateShoppingCartDto, ShoppingCart>().ReverseMap();
        }
    }
}
