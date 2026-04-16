namespace Clothify.Application.DTOs.Category
{
    public class UpdateCategoryDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
