namespace Clothify.Domain.Entities
{
    public class ShoppingCart
    {
        public Guid CartId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public Guid UserId { get; set; }
    }
}
