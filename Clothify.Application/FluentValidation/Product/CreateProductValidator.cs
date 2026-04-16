using Clothify.Application.DTOs.Product;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Product
{
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(200);
            RuleFor(p => p.Description).NotEmpty().MaximumLength(1000);
            RuleFor(p => p.Price).GreaterThan(0);
            RuleFor(p => p.Stock).GreaterThanOrEqualTo(0);
            RuleFor(p => p.Color).NotEmpty();
            RuleFor(p => p.ImageUrl).NotEmpty();
            RuleFor(p => p.BrandId).NotEmpty();
            RuleFor(p => p.CategoryId).NotEmpty();
        }
    }
}
