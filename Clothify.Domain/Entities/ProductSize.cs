namespace Clothify.Domain.Entities
{
    public class ProductSize
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid SizeId { get; set; }
        public Size Size { get; set; }
    }
}
