using AutoMapper;
using Clothify.Application.DTOs.OrderItem;
using Clothify.Domain.Entities;

namespace Clothify.Application.Mapping
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItemDto, OrderItem>().ReverseMap();
            CreateMap<CreateOrderItemDto, OrderItem>().ReverseMap();
            CreateMap<UpdateOrderItemDto, OrderItem>().ReverseMap();
        }
    }
}
