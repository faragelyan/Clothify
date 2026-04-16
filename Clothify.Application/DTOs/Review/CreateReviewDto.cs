using System;

namespace Clothify.Application.DTOs.Review
{
    public class CreateReviewDto
    {
        public byte Rating { get; set; }
        public string? Comment { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
}
