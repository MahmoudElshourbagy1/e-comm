using Ecom.Core.Entites.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecomm.infrastructure.Data.Config
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(m => m.Price).HasColumnType("decimal(18,2)");
            builder.HasData(new DeliveryMethod { Id = 1, DeliveryTime = "Only a week", Description = "the fast Delivery in the world", Name = "DHL", Price = 60 });
            builder.HasData(new DeliveryMethod { Id = 2, DeliveryTime = "Only a 2week", Description = "the fast Delivery in the world", Name = "XXX", Price = 30 });

        }
    }
}
