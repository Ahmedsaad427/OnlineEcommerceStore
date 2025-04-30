using Domain.Models.OrderModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Data.Configurations
    {
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
        {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
            {
            builder.OwnsOne(P => P.Product, item => item.WithOwner());

            builder.Property(P => P.Price)
                .HasColumnType("decimal(18,2)");
            }
        }
    }
