using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities.BasketModule;
using Talabat.Core.Entities.Order_aggregate;
using Talabat.Core.Entities.ProductModule;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specification;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketRepository basketRepository;

        public OrderService(IUnitOfWork unitOfWork , IBasketRepository basketRepository)
        {
            this.unitOfWork = unitOfWork;
            this.basketRepository = basketRepository;
        }
        public async Task<Order?> CreateOrder(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            var basket = await basketRepository.GetBasketAsync(basketId);
            var OrderItems = new List<OrderItem>();
            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                    OrderItems.Add(new OrderItem
                    {
                        Product = productItemOrdered,
                        Price = product.Price,
                        Quantity = item.Quantity
                    });
                }
            }
            var subTotal = OrderItems.Sum(O=>O.Price * O.Quantity);
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            var Order = new Order(buyerEmail, shippingAddress, OrderItems, deliveryMethod, subTotal);
            await unitOfWork.Repository<Order>().AddAsync(Order);
            var result = await unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return Order;
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUser(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail);
            return unitOfWork.Repository<Order>().GetAllAsyncWithSpec(spec);
        }

        public Task<Order> GetOrderByIdForUser(string buyerEmail, int id)
        {
            var spec = new OrderSpecification(id, buyerEmail);
            return unitOfWork.Repository<Order>().GetByIdAsyncWithSpec(spec);
        }

    }
}
