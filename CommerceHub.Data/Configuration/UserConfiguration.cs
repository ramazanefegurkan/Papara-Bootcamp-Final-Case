using CommerceHub.Data.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(x => x.InsertDate)
                .IsRequired();
            
            builder.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.DateOfBirth)
                .IsRequired();

            builder.Property(e => e.Role)
                .IsRequired();

            builder.Property(e => e.WalletBalance)
                .IsRequired();

            builder.HasMany(u => u.CreatedCoupons)
                .WithOne(c => c.AdminUser)
                .HasForeignKey(c => c.AdminUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
