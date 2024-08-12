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
    public class CategoryProductConfiguration : IEntityTypeConfiguration<CategoryProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryProduct> builder)
        {
            builder.HasKey(e => new { e.CategoryId, e.ProductId });

            builder.HasOne(e => e.Category)
                .WithMany(c => c.CategoryProducts)
                .HasForeignKey(e => e.CategoryId);

            builder.HasOne(e => e.Product)
                .WithMany(p => p.CategoryProducts)
                .HasForeignKey(e => e.ProductId);
        }
    }
}
