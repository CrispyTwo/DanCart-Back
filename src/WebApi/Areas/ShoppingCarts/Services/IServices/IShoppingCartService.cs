using DanCart.DataAccess.Models.Utility;
using DanCart.WebApi.Areas.Products.DTOs;
using DanCart.WebApi.Areas.ShoppingCarts.DTOs;
using FluentResults;

namespace DanCart.WebApi.Areas.ShoppingCarts.Services.IServices;

public interface IShoppingCartService
{
    Task<Result> AddAsync(string userId, Guid productId, ProductVariant variant, int quantity);
    Task<Result> AddAsync(string userId, Guid productId, ProductVariant variant);
    Task<Result> DeleteAsync(string userId, Guid productId, ProductVariant variant);
    Task<Result> DeleteAsync(string userId);
    Task<Result<ShoppingCartDTO>> GetAsync(string userId, Page page, string? sort);
}
