using Clothify.Application.DTOs.ProductSize;
using FluentValidation;

namespace Clothify.Application.FluentValidation.ProductSize
{
    public class CreateProductSizeValidator : AbstractValidator<CreateProductSizeDto>
    {
        public CreateProductSizeValidator()
        {
            RuleFor(ps => ps.ProductId).NotEmpty();
            RuleFor(ps => ps.SizeId).NotEmpty();
        }
    }
}
