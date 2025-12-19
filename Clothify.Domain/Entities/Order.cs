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
        public Guid AddressId { get; set; }
    }
}
