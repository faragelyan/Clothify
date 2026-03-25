using Clothify.Application.DTOs.Brand;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Brand
{
    public class UpdateBrandValidator : AbstractValidator<UpdateBrandDto>
    {
        public UpdateBrandValidator()
        {
            RuleFor(b => b.BrandId)
                .NotEmpty();

            RuleFor(b => b.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(b => b.Description)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
