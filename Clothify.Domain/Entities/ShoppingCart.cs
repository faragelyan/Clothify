namespace Clothify.Domain.Entities
{
    public class ShoppingCart
    {
        public Guid CartID { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
