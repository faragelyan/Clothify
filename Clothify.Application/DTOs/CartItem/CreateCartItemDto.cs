namespace Clothify.Application.DTOs.CartItem
{
    public class CreateCartItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
