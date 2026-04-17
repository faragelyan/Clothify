using AutoMapper;
using Clothify.Application.DTOs.Payment;

namespace Clothify.Application.Mapping
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
            CreateMap<Domain.Entities.Payment, PaymentDto>().ReverseMap();
            CreateMap<Domain.Entities.Payment, CreatePaymentDto>().ReverseMap();
            CreateMap<Domain.Entities.Payment, UpdatePaymentDto>().ReverseMap();
        }
    }
}
