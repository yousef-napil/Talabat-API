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
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(O=>O.Status)
                   .HasConversion(
                       o => o.ToString(),
                       o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
                   );
            builder.OwnsOne(O => O.ShipingAddress, SA => SA.WithOwner());
            builder.Property(O => O.SubTotal)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(O => O.DeliveryMethod)
                   .WithMany()
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
