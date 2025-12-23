using Microsoft.EntityFrameworkCore;
using Clothify.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Clothify.Infrastructure.Configurations
{
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.AddressId);

            builder.Property(a => a.FullAddress).IsRequired();
            builder.Property(a => a.AddressType).IsRequired();

            builder.HasMany(a => a.Orders)            
                   .WithOne(o => o.Address)
                   .HasForeignKey(o => o.AddressId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => a.UserId);
        }
    }
}
