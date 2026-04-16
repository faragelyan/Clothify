using System;

namespace Clothify.Application.DTOs.ShoppingCart
{
    public class ShoppingCartDto
    {
        public Guid CartId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid UserId { get; set; }
    }
}
