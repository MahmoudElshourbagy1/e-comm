using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDTO>().ForMember(x=>x.CategoryName,op=>op.MapFrom(src=>src.Category.Name))
                .ReverseMap();
            CreateMap<Photo, PhotoDTO>().ReverseMap();
            CreateMap<AddProductDTO, Product>().ForMember(m=>m.Photos,op=>op.Ignore()).ReverseMap();
            CreateMap<UpdateProductDTO, Product>().ForMember(m => m.Photos, op => op.Ignore()).ReverseMap();
        }
    }
}
