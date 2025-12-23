using Clothify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clothify.Infrastructure.Configurations
{
    public class UserPhoneConfig : IEntityTypeConfiguration<UserPhone>
    {
        public void Configure(EntityTypeBuilder<UserPhone> builder)
        {
            builder.HasKey(p => p.PhoneId);

            builder.Property(p => p.PhoneNumber).IsRequired();
            builder.Property(p => p.Type).IsRequired();

            builder.HasIndex(p => p.UserId);
        }
    }
}
