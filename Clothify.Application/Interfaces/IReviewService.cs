using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Review;

namespace Clothify.Application.Interfaces
{
    public interface IReviewService
    {
        Task<Result<Guid>> AddAsync(CreateReviewDto dto);
        Task<Result<bool>> UpdateAsync(UpdateReviewDto dto);
        Task<Result<bool>> RemoveAsync(Guid reviewId);
        Task<Result<IReadOnlyList<ReviewDto>>> GetAllByProductIdAsync(Guid productId);
        Task<Result<ReviewDto>> GetAsync(Guid reviewId);
    }
}
