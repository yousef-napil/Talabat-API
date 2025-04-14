using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_aggregate;

namespace Talabat.Core.Specification
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(string buyerEmail) : base(o => o.BuyerEmail == buyerEmail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            Includes.Add(o => o.ShipingAddress);
        }
        public OrderSpecification(int id, string buyerEmail) : base(o => o.Id == id && o.BuyerEmail == buyerEmail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            Includes.Add(o => o.ShipingAddress);
        }
    }
}
