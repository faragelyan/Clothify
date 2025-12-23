using Clothify.Domain.Enums;

namespace Clothify.Domain.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }

        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid AddressId { get; set; }
        public Address Address { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
