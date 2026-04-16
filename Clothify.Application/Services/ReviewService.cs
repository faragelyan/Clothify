using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Review;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;

namespace Clothify.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> AddAsync(CreateReviewDto dto)
        {
            var exists = await _unitOfWork.Reviews.GetCountAsync(
                filter: r => r.UserId == dto.UserId && r.ProductId == dto.ProductId
            );

            if (exists > 0)
                return Result<Guid>.Fail("User has already reviewed this product.");

            var review = _mapper.Map<Review>(dto);
            review.ReviewDate = DateTime.UtcNow;

            var added = await _unitOfWork.Reviews.AddAsync(review);
            if (!added)
                return Result<Guid>.Fail("Failed to add review.");

            await _unitOfWork.CommitAsync();
            return Result<Guid>.Ok(review.ReviewId);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateReviewDto dto)
        {
            var review = await _unitOfWork.Reviews.GetSingleEntityAsync(
                filter: r => r.ReviewId == dto.ReviewId
            );

            if (review is null)
                return Result<bool>.Fail("Review not found");

            _mapper.Map(dto, review);

            var updated = _unitOfWork.Reviews.Update(review);
            if (!updated)
                return Result<bool>.Fail("Failed to update review.");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> RemoveAsync(Guid reviewId)
        {
            var review = await _unitOfWork.Reviews.GetSingleEntityAsync(
                filter: r => r.ReviewId == reviewId
            );

            if (review is null)
                return Result<bool>.Fail("Review not found");

            var deleted = _unitOfWork.Reviews.Delete(review);
            if (!deleted)
                return Result<bool>.Fail("Failed to delete review.");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<IReadOnlyList<ReviewDto>>> GetAllByProductIdAsync(Guid productId)
        {
            var reviews = await _unitOfWork.Reviews.GetAllEntitiesAsync(
                filter: r => r.ProductId == productId,
                orderBy: q => q.OrderByDescending(r => r.ReviewDate),
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<ReviewDto>>(reviews);
            return Result<IReadOnlyList<ReviewDto>>.Ok(result);
        }

        public async Task<Result<ReviewDto>> GetAsync(Guid reviewId)
        {
            var review = await _unitOfWork.Reviews.GetSingleEntityAsync(
                filter: r => r.ReviewId == reviewId,
                disableTracking: true
            );

            if (review is null)
                return Result<ReviewDto>.Fail("Review not found");

            var dto = _mapper.Map<ReviewDto>(review);
            return Result<ReviewDto>.Ok(dto);
        }
    }
}
