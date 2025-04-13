using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTOs;
using Talabat.API.DTOs.Identity;
using Talabat.API.Errors;
using Talabat.Core.Entities.Order_aggregate;
using Talabat.Core.Services;

namespace Talabat.API.Controllers
{

    public class OrdersController : APIBaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orderService ,
                                IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var mappedAddress = mapper.Map<AddressDto, Address>(orderDto.ShipingAddress);
            var order = await orderService.CreateOrder(buyerEmail, orderDto.DeliveryMethodId, orderDto.BasketId, mappedAddress);
            if (order == null) return BadRequest(new ApiResponse(400, "There is problem in your Order"));
            return Ok(order);
        }
    }
}
