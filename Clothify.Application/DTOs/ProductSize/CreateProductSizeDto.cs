using System;

namespace Clothify.Application.DTOs.ProductSize
{
    public class CreateProductSizeDto
    {
        public Guid ProductId { get; set; }
        public Guid SizeId { get; set; }
    }
}
