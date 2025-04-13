using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_aggregate;

namespace Talabat.Repository.Data.Configurations.OrderConfiguration
{
    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(p=>p.Product , OI=>OI.WithOwner());
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }
    }

}
