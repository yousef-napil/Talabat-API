using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_aggregate
{
    public class Order : BaseEntity
    {
        public Order() { }
        public Order(string buyerEmail, Address shipingAddress, ICollection<OrderItem> items, DeliveryMethod deliveryMethod, decimal subTotal)
        {
            BuyerEmail = buyerEmail;
            ShipingAddress = shipingAddress;
            Items = items;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
        }

        public string BuyerEmail { get; set; }
        public Address ShipingAddress { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DeliveryMethod DeliveryMethod { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public decimal SubTotal { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;

        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;

    }
    
}
