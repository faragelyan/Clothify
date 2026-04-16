using System;

namespace Clothify.Application.DTOs.Review
{
    public class UpdateReviewDto
    {
        public Guid ReviewId { get; set; }
        public byte Rating { get; set; }
        public string? Comment { get; set; }
    }
}
