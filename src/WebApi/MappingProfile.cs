using AutoMapper;
using DanCart.Models.Auth;
using DanCart.Models.Products;
using DanCart.Models.SalesOrders;
using DanCart.Products.Models.DTOs;
using DanCart.WebApi.Areas.Auth.DTOs;
using DanCart.WebApi.Areas.Products.DTOs;
using DanCart.WebApi.Areas.SalesOrders.DTOs;
using DanCart.WebApi.Areas.SalesOrders.SalesLines.DTOs;

namespace DanCart.WebApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegisterDTO, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<ProductCreateDTO, Product>();
        CreateMap<ProductUpdateDTO, Product>();
        CreateMap<Product, ProductDTO>();
        CreateMap<Product, ProductWithImagesDTO>();

        CreateMap<SalesOrderCreateDTO, SalesOrder>();
        CreateMap<SalesOrderUpdateDTO, SalesOrder>();
        CreateMap<SalesOrder, SalesOrderWithLinesDTO>();
        CreateMap<SalesLineCreateDTO, SalesLine>();
        CreateMap<SalesLineUpdateDTO, SalesLine>();
    }
}
