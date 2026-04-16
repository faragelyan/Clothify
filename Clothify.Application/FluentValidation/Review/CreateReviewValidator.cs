using Clothify.Application.DTOs.Review;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Review
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewDto>
    {
        public CreateReviewValidator()
        {
            RuleFor(r => r.UserId).NotEmpty();
            RuleFor(r => r.ProductId).NotEmpty();
            RuleFor(r => r.Rating).InclusiveBetween((byte)1, (byte)5);
            RuleFor(r => r.Comment).MaximumLength(1000);
        }
    }
}
