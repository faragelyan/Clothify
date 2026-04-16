using Clothify.Application.DTOs.Category;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Category
{
    public class CategoryValidator : AbstractValidator<CategoryDto>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(c => c.Description)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
