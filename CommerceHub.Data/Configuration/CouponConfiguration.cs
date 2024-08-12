using CommerceHub.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.Configuration
{
    internal class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(c => c.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(c => c.ExpiryDate)
                .IsRequired();

            builder.HasOne(c => c.AdminUser)
                .WithMany()
                .HasForeignKey(c => c.AdminUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.UsedOrders)
                .WithOne(o => o.UsedCoupon)
                .HasForeignKey(o => o.CouponId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
