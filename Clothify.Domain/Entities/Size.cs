namespace Clothify.Domain.Entities
{
    public class Size
    {
        public Guid SizeId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}
