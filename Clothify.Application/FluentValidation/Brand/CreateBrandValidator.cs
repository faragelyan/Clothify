using Clothify.Application.DTOs.Brand;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Brand
{
    public class CreateBrandValidator : AbstractValidator<CreateBrandDto>
    {
        public CreateBrandValidator()
        {
            RuleFor(b => b.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(b => b.Description)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
