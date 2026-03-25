using AutoMapper;
using Clothify.Application.DTOs.Address;
using Clothify.Domain.Entities;

namespace Clothify.Application.Mapping
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<UpdateAddressDto, Address>().ReverseMap();
            CreateMap<CreateAddressDto, Address>().ReverseMap();
        }
    }
}