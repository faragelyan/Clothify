using Clothify.Domain.Enums;

namespace Clothify.Domain.Entities
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public string FullAddress { get; set; } = null!;
        public AddressType AddressType { get; set; } 

        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
