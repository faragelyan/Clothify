using Clothify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clothify.Infrastructure.Configurations
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.PaymentId);

            builder.Property(p => p.Amount).IsRequired().HasPrecision(18, 2);
            builder.Property(p => p.Currency).IsRequired();
            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.PaymentDate).IsRequired();
            builder.Property(p => p.PaymentMethod).IsRequired();

            builder.HasIndex(p => p.OrderId);
        }
    }
}
