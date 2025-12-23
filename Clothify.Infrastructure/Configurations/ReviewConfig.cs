using Clothify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clothify.Infrastructure.Configurations
{
    public class ReviewConfig : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.ReviewId);

            builder.Property(r => r.Rating).IsRequired();
            builder.Property(r => r.Comment).IsRequired(false);
            builder.Property(r => r.ReviewDate).IsRequired();

            builder.HasIndex(r => r.UserId);
            builder.HasIndex(r => r.ProductId);
        }
    }
}
