using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites;
using Ecom.Core.Entites.Order;

namespace Ecom.API.Controllers.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<Orders,OrderToReturnDTO>()
                .ForMember(d=>d.deliveryMethod,
                o=>o.MapFrom(s=>s.deliveryMethod.Name)).ReverseMap();
            CreateMap<OrderItem,OrderItemDTO>().ReverseMap();
            CreateMap<ShippingAddress,ShipAddressDTO>().ReverseMap();
            CreateMap<Address, ShipAddressDTO>().ReverseMap();
        }
    }
}
