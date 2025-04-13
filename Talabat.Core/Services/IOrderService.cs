using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_aggregate;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrder(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForUser(string buyerEmail);
        Task<Order> GetOrderByIdForUser(string buyerEmail , int id);
    }
}
