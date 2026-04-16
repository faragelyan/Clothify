using System;

namespace Clothify.Application.DTOs.Size
{
    public class UpdateSizeDto
    {
        public Guid SizeId { get; set; }
        public string Name { get; set; } = null!;
    }
}
