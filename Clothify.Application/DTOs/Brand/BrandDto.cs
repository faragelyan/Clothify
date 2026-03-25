namespace Clothify.Application.DTOs.Brand
{
    public class BrandDto
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
