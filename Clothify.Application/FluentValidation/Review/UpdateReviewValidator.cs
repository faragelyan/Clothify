using Clothify.Application.DTOs.Review;
using FluentValidation;

namespace Clothify.Application.FluentValidation.Review
{
    public class UpdateReviewValidator : AbstractValidator<UpdateReviewDto>
    {
        public UpdateReviewValidator()
        {
            RuleFor(r => r.ReviewId).NotEmpty();
            RuleFor(r => r.Rating).InclusiveBetween((byte)1, (byte)5);
            RuleFor(r => r.Comment).MaximumLength(1000);
        }
    }
}
