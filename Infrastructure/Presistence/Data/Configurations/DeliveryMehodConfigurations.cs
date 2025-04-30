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
    public class DeliveryMehodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
        {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
            {

            builder.Property(P => P.Cost)
                .HasColumnType("decimal(18,4)");

            }
        }
    }
