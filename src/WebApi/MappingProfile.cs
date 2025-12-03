using AutoMapper;
using DanCart.Models.Auth;
using DanCart.Models.Products;
using DanCart.Models.SalesOrders;
using DanCart.Products.Models.DTOs;
using DanCart.WebApi.Areas.Auth.DTOs;
using DanCart.WebApi.Areas.Customers.DTOs.Metrics;
using DanCart.WebApi.Areas.Products.DTOs;
using DanCart.WebApi.Areas.SalesOrders.DTOs;
using DanCart.WebApi.Areas.SalesOrders.SalesLines.DTOs;
using DanCart.WebApi.Areas.ShoppingCarts.DTOs;

namespace DanCart.WebApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegisterRequest, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<ApplicationUser, CustomerWithSalesInfoResponse>()
            .ForMember(d => d.OrdersCount, o => o.MapFrom(s => s.SalesOrders.LongCount()))
            .ForMember(d => d.TotalSpent, o => o.MapFrom(s => 
                s.SalesOrders.SelectMany(sl => sl.SalesLines).Sum(x => x.Quantity * x.UnitPrice)));

        CreateMap<ProductCreateDTO, Product>();
        CreateMap<ProductUpdateDTO, Product>();
        CreateMap<Product, ProductDTO>();
        CreateMap<Product, ProductWithImagesDTO>();

        CreateMap<SalesOrderCreateDTO, SalesOrder>();
        CreateMap<SalesOrderUpdateDTO, SalesOrder>();
        CreateMap<SalesOrder, SalesOrderWithLinesDTO>()
            .ForMember(d => d.Lines, o => o.MapFrom(s => s.SalesLines))
            .ForMember(d => d.Total, o => o.MapFrom(s => s.SalesLines.Sum(x => x.Quantity * x.UnitPrice)));

        CreateMap<SalesLineCreateDTO, SalesLine>();
        CreateMap<SalesLineUpdateDTO, SalesLine>();
        CreateMap<SalesLine, SalesLineDTO>();

        CreateMap<Product, SalesLine>();
        CreateMap<IEnumerable<ShoppingCart>, SalesOrder>()
            .ForMember(dest => dest.SalesLines, opt => opt.MapFrom(src => src));

        CreateMap<ShoppingCart, SalesLine>();
        CreateMap<ShoppingCart, CartItemDTO>();
        CreateMap<IEnumerable<ShoppingCart>, ShoppingCartDTO>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src));
    }
}
