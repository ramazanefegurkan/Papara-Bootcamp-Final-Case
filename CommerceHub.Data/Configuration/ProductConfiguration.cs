using CommerceHub.Data.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace CommerceHub.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.Property(e => e.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.RewardPointsPercentage)
                .HasColumnType("decimal(5,2)");

            builder.Property(e => e.MaxRewardPoints)
                .HasColumnType("decimal(18,2)");

            builder.HasMany(p => p.CategoryProducts)
                .WithOne(cp => cp.Product)
                .HasForeignKey(cp => cp.ProductId);

            builder.HasMany(p => p.OrderDetails)
                .WithOne(od => od.Product)
                .HasForeignKey(od => od.ProductId);
        }
    }
}
