using Talabat.API.DTOs.Identity;

namespace Talabat.API.DTOs
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto ShipingAddress { get; set; }
    }
}
