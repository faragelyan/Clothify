using Clothify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clothify.Infrastructure.Configurations
{
    public class SizeConfig : IEntityTypeConfiguration<Size>
    {
        public void Configure(EntityTypeBuilder<Size> builder)
        {
            builder.HasKey(s => s.SizeId);
            builder.Property(s => s.Name).IsRequired();

            builder.HasMany(s => s.ProductSizes)
                   .WithOne(ps => ps.Size)
                   .HasForeignKey(ps => ps.SizeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
