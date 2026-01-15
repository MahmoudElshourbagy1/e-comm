using Ecom.Core.Entites.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecomm.infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);
            builder
                .Property(x => x.NewPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.
                Property(x => x.OldPrice).
                HasColumnType("decimal(18,2)");
            builder
                .HasData(
                    new Product
                    {
                        Id = 1,
                        Name = "Sample Product 1",
                        Description = "This is a sample product description.",
                        NewPrice = 19.99m,
                        CategoryId = 1
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Sample Product 2",
                        Description = "This is another sample product description.",
                        NewPrice = 29.99m,
                        CategoryId = 2
                    },
                     new Product
                     {
                         Id = 3,
                         Name = "Sample Product 3",
                         Description = "This is another sample product description.",
                         NewPrice = 35.99m,
                         CategoryId = 3
                     }
                );
        }
    }
}
