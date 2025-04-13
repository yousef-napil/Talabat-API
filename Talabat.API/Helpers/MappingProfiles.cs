using AutoMapper;
using Talabat.API.DTOs;
using Talabat.API.DTOs.Identity;
using Talabat.Core.Entities.BasketModule;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_aggregate;
using Talabat.Core.Entities.ProductModule;

namespace Talabat.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Product, ProductToReturnDto>()
                    .ForMember(d => d.ProductBrand , o => o.MapFrom(p => p.ProductBrand.Name))
                    .ForMember(d => d.ProductType , o=> o.MapFrom(p => p.ProductType.Name))
                    .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureResolver>());
            CreateMap<Core.Entities.Identity.Address , AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem >();
            CreateMap<AddressDto , Core.Entities.Order_aggregate.Address>();
        }
    }
}
