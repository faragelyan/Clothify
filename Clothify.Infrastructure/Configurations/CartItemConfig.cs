using Clothify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clothify.Infrastructure.Configurations
{
    public class CartItemConfig : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(ci => new { ci.CartId, ci.ProductId });

            builder.Property(ci => ci.Quantity).IsRequired();
            builder.Property(ci => ci.AddedAt).IsRequired();

            builder.HasIndex(ci => ci.ProductId);
        }
    }
}
