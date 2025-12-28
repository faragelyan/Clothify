using Clothify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clothify.Infrastructure.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.Price).IsRequired().HasPrecision(18, 2);
            builder.Property(p => p.Stock).IsRequired();
            builder.Property(p => p.Color).IsRequired();
            builder.Property(p => p.ImageUrl).IsRequired();
            builder.Property(p => p.CreatedAt).IsRequired();

            builder.HasMany(p => p.Reviews)
                   .WithOne(r => r.Product)
                   .HasForeignKey(r => r.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.ProductSizes)
                   .WithOne(ps => ps.Product)
                   .HasForeignKey(ps => ps.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.CartItems)
                   .WithOne(ci => ci.Product)
                   .HasForeignKey(ci => ci.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.OrderItems)
                   .WithOne(oi => oi.Product)
                   .HasForeignKey(oi => oi.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.CategoryId);
            builder.HasIndex(p => p.BrandId);
            builder.HasIndex(p => p.Price);
            builder.HasIndex(p => p.CreatedAt);
        }
    }
}
