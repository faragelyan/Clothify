namespace Clothify.Domain.Entities
{
    public class Brand
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
