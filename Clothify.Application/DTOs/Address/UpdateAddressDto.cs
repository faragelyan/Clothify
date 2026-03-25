using Clothify.Domain.Enums;

namespace Clothify.Application.DTOs.Address
{
    public class UpdateAddressDto
    {
        public Guid AddressId { get; set; }
        public string FullAddress { get; set; } = null!;
        public AddressType AddressType { get; set; }
    }
}
