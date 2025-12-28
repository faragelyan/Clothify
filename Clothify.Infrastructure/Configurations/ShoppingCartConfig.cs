using Clothify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clothify.Infrastructure.Configurations
{
    public class ShoppingCartConfig : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.HasKey(c => c.CartId);

            builder.Property(c => c.CreatedAt).IsRequired();
            builder.Property(c => c.TotalAmount).IsRequired().HasPrecision(18, 2);

            builder.HasOne(c => c.AppUser)
               .WithOne()
               .HasForeignKey<ShoppingCart>(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.CartItems)
                   .WithOne(ci => ci.ShoppingCart)
                   .HasForeignKey(ci => ci.CartId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
