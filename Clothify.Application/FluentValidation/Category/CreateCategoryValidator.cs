using Clothify.Application.DTOs.Category;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Category
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
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
