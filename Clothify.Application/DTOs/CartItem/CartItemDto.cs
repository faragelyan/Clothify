namespace Clothify.Application.DTOs.CartItem
{
    public class CartItemDto
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }

        public string? ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public string? ProductImageUrl { get; set; }
    }
}
