using Clothify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clothify.Infrastructure.Configurations
{
    public class PendingVerificationConfig : IEntityTypeConfiguration<PendingVerification>
    {
        public void Configure(EntityTypeBuilder<PendingVerification> builder)
        {
            builder.ToTable("PendingVerifications");
            builder.HasKey(pv => pv.Id);
            builder.Property(pv => pv.Id).ValueGeneratedOnAdd();
            builder.Property(pv => pv.Id).IsRequired();

            builder.Property(pv => pv.Email).IsRequired();

            builder.Property(pv => pv.VerificationCode).IsRequired();

            builder.Property(pv => pv.Expiry).IsRequired();

            builder.Property(pv => pv.IsConfirmed).IsRequired().HasDefaultValue(false);

        }
    }
}
