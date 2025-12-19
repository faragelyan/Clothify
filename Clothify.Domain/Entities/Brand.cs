namespace Clothify.Domain.Entities
{
    internal class Brand
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
