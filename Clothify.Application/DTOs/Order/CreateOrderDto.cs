using Clothify.Domain.Enums;

namespace Clothify.Application.DTOs.Order
{
    public class CreateOrderDto
    {
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid UserId { get; set; }
        public Guid AddressId { get; set; }
    }
}
