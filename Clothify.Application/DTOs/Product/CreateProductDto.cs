using System;

namespace Clothify.Application.DTOs.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Color { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
