using Clothify.Domain.Enums;

namespace Clothify.Application.DTOs.Address
{
    public class CreateAddressDto
    {
        public string FullAddress { get; set; } = null!;
        public AddressType AddressType { get; set; }
    }
}
