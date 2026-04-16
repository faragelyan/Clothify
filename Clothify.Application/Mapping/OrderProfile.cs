using AutoMapper;
using Clothify.Application.DTOs.Order;

namespace Clothify.Application.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderDto, Clothify.Domain.Entities.Order>().ReverseMap();
            CreateMap<CreateOrderDto, Clothify.Domain.Entities.Order>().ReverseMap();
            CreateMap<UpdateOrderDto, Clothify.Domain.Entities.Order>().ReverseMap();
        }
    }
}
