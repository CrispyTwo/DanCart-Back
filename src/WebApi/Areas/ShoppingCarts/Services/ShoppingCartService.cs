using AutoMapper;
using AutoMapper.QueryableExtensions;
using DanCart.DataAccess.Extensions;
using DanCart.DataAccess.Models.Utility;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models.Products;
using DanCart.Models.SalesOrders;
using DanCart.WebApi.Areas.Products.DTOs;
using DanCart.WebApi.Areas.Products.Services.IServices;
using DanCart.WebApi.Areas.ShoppingCarts.DTOs;
using DanCart.WebApi.Areas.ShoppingCarts.Services.IServices;
using DanCart.WebApi.Core;
using FluentResults;

namespace DanCart.WebApi.Areas.ShoppingCarts.Services;

public class ShoppingCartService(IUnitOfWork _unitOfWork, IMapper _mapper, IProductsBlobService _blobService) : ServiceBase, IShoppingCartService
{
    public async Task<Result> AddAsync(string userId, Guid productId, ProductVariant variant, int quantity)
    {
        var product = await _unitOfWork.Product.GetAsync(x => x.Id == productId);
        if (product == null) return DefaultNotFound(productId, nameof(Product));

        var entity = await _unitOfWork.ShoppingCart.GetAsync(x => x.UserId == userId && x.ProductId == productId && x.Size == variant.Size && x.Color == variant.Color, tracked: true);
        if (entity != null)
        {
            entity.Quantity = quantity;
        }
        else
        {
            var cart = new ShoppingCart() { UserId = userId, ProductId = productId, Color = variant.Color, Size = variant.Size, Quantity = quantity };
            await _unitOfWork.ShoppingCart.AddAsync(cart);
        }

        await _unitOfWork.SaveAsync();
        return Result.Ok();
    }

    public async Task<Result> AddAsync(string userId, Guid productId, ProductVariant variant)
    {
        var product = await _unitOfWork.Product.GetAsync(x => x.Id == productId);
        if (product == null) return DefaultNotFound(productId, nameof(Product));

        var entity = await _unitOfWork.ShoppingCart.GetAsync(x => x.UserId == userId && x.ProductId == productId && x.Size == variant.Size && x.Color == variant.Color, tracked: true);
        if (entity != null)
        {
            entity.Quantity++;
        }
        else
        {
            var cart = new ShoppingCart() { UserId = userId, ProductId = productId, Color = variant.Color, Size = variant.Size, Quantity = 1 };
            await _unitOfWork.ShoppingCart.AddAsync(cart);
        }

        await _unitOfWork.SaveAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(string userId, Guid productId, ProductVariant variant)
    {
        var entity = await _unitOfWork.ShoppingCart.GetAsync(x => x.UserId == userId && x.ProductId == productId && x.Size == variant.Size && x.Color == variant.Color);
        if (entity == null) return DefaultNotFound(productId, nameof(Product));

        _unitOfWork.ShoppingCart.Remove(entity);
        await _unitOfWork.SaveAsync();

        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(string userId)
    {
        var entities = await _unitOfWork.ShoppingCart.GetQuery().Where(x => x.UserId == userId).GetAllAsync();
        if (entities != null) _unitOfWork.ShoppingCart.RemoveRange(entities);
        await _unitOfWork.SaveAsync();
        return Result.Ok();
    }

    public async Task<Result<ShoppingCartDTO>> GetAsync(string userId, Page page, string? sort)
    {
        const int MinPageSize = 10, MaxPageSize = 25;
        page.ApplySizeRule(MinPageSize, MaxPageSize);
        var smth = await _unitOfWork.ShoppingCart.GetQuery().GetPageAsync(page);
        var records = await _unitOfWork.ShoppingCart.GetQuery()
            .Where(x => x.UserId == userId)
            .ProjectTo<CartItemDTO>(_mapper.ConfigurationProvider)
            .GetPageAsync(page, BuildSortingMap(sort));

        foreach (var item in records)
        {
            item.Product = _blobService.AttachImages(item.Product);
        }

        return Result.Ok(new ShoppingCartDTO() { Items = records });
    }
}
