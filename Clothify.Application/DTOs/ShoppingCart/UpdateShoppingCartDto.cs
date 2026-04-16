using System;

namespace Clothify.Application.DTOs.ShoppingCart
{
    public class UpdateShoppingCartDto
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
