namespace Clothify.Domain.Entities
{
    public class CartItem
    {
        public Guid CartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; } = null!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
