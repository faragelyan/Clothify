using System;

namespace Clothify.Application.DTOs.OrderItem
{
    public class CreateOrderItemDto
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
