using Clothify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clothify.Infrastructure.Configurations
{
    public class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.CreatedAt).IsRequired();
            builder.Property(u => u.ProfileImageUrl).HasMaxLength(512);
            builder.Property(u => u.DateOfBirth)
                    .HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue),
                    v => DateOnly.FromDateTime(v))
                    .IsRequired();
            builder.Property(u => u.RefreshToken)
                   .HasMaxLength(500)
                   .IsUnicode(false)
                   .IsRequired(false);
            builder.Property(u => u.RefreshTokenExpiryTime)
                   .IsRequired(false);


            builder.HasMany(u => u.UserPhones)
                   .WithOne(p => p.AppUser)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Addresses)
                   .WithOne(a => a.AppUser)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Orders)
                   .WithOne(o => o.AppUser)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Reviews)
                   .WithOne(r => r.AppUser)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Reports)
                   .WithOne(r => r.AppUser)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(u => u.Email).IsUnique();

        }
    }
}
