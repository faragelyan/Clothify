namespace Clothify.Application.DTOs.CartItem
{
    public class UpdateCartItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
