using Clothify.Application.DTOs.Category;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Category
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(c => c.CategoryId)
                .NotEmpty();

            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(c => c.Description)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
