using AutoMapper;
using DanCart.Models;
using DanCart.Models.DTOs.Authentication;
using DanCart.Models.DTOs.Product;
using DanCart.Models.DTOs.SalesLine;
using DanCart.Models.DTOs.SalesOrder;

namespace DanCart.WebApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegisterDTO, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<ProductCreateDTO, Product>();
        CreateMap<ProductUpdateDTO, Product>();

        CreateMap<SalesOrderCreateDTO, SalesOrder>();
        CreateMap<SalesOrderUpdateDTO, SalesOrder>();
        CreateMap<SalesLineCreateDTO, SalesLine>();
        CreateMap<SalesLineUpdateDTO, SalesLine>();
    }
}
