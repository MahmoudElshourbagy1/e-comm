using Ecom.Core.Entites.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecomm.infrastructure.Data.Config
{
    public class CategotyConfiguration :IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(30);
            builder
                .Property(x => x.Id)
                .IsRequired();
            builder
                .HasData(
                    new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets" },
                    new Category { Id = 2, Name = "Books", Description = "Various kinds of books" },
                    new Category { Id = 3, Name = "Clothing", Description = "Apparel and garments" }
                );
         }
    }  
}
