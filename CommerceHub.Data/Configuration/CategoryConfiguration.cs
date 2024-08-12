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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(e => e.Url)
                    .HasMaxLength(200);

            builder.Property(e => e.Tags)
                    .HasMaxLength(500);

            builder.HasMany(c => c.CategoryProducts)
                .WithOne(cp => cp.Category)
                .HasForeignKey(cp => cp.CategoryId);
        }
    }
}
