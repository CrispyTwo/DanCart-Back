using AutoMapper;
using DanCart.Models;
using DanCart.Models.DTOs.Authentication;
using DanCart.Models.DTOs.Product;

namespace DanCart.WebApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegisterDTO, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<ProductCreateDTO, Product>();
        CreateMap<ProductUpdateDTO, Product>();
    }
}
