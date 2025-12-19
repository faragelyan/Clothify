namespace Clothify.Domain.Entities
{
    public class CartItem
    {
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int AddedAt { get; set; }
    }
}
