using AutoMapper;
using DanCart.DataAccess.Blob;
using DanCart.DataAccess.Extensions;
using DanCart.DataAccess.Models;
using DanCart.DataAccess.Models.Utility;
using DanCart.DataAccess.Repository;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models.Auth;
using DanCart.Models.Products;
using DanCart.Products.Models.DTOs;
using DanCart.WebApi.Areas.Products.DTOs;
using DanCart.WebApi.Areas.Products.Services.IServices;
using DanCart.WebApi.Core;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DanCart.WebApi.Areas.Products.Services;

public class ProductsService(IUnitOfWork _unitOfWork, IMapper _mapper, IBlobService _blobService) : ServiceBase, IProductsService
{
    public async Task<Result<IEnumerable<ProductDTO>>> GetAsync(Page page, ProductStockStatus? status, string? priceRange, string[]? categories, string? sort, string? search)
    {
        const int MinPageSize = 10, MaxPageSize = 50;
        page.ApplySizeRule(MinPageSize, MaxPageSize);

        var query = _unitOfWork.Product.GetQuery();
        switch (status)
        {
            case ProductStockStatus.InStock:
                query = query.Where(x => x.Stock > x.LowStockThreshold);
                break;
            case ProductStockStatus.LowStock:
                query = query.Where(x => x.Stock <= x.LowStockThreshold && x.Stock > 0);
                break;
            case ProductStockStatus.OutOfStock:
                query = query.Where(x => x.Stock <= 0);
                break;
        }

        if (!string.IsNullOrWhiteSpace(priceRange))
        {
            var ranges = priceRange.Split('-');
            if (ranges.Length < 2)
            {
                var range = decimal.Parse(ranges[0]);
                if (priceRange[0] == '-')
                    query = query.Where(x => x.Price <= range);
                else
                    query = query.Where(x => x.Price >= range);
            }
            else
            {
                query = query.Where(x => x.Price >= decimal.Parse(ranges[0]) && x.Price <= decimal.Parse(ranges[1]));
            }
        }

        if (categories != null && categories.Length != 0)
        {
            query = query.Where(x => categories.Contains(x.Category));
        }

        var products = await query.GetPageAsync(page, BuildSortingMap(sort), search);
        var result = _mapper.Map<IEnumerable<ProductDTO>>(products);
        return Result.Ok(result);
    }

    public async Task<Result<ProductWithImagesDTO>> GetByIdAsync(Guid id)
    {
        var product = await _unitOfWork.Product.GetAsync(x => x.Id == id);
        if (product == null) return DefaultNotFound<ProductWithImagesDTO>(id);

        var result = _mapper.Map<ProductWithImagesDTO>(product);
        return Result.Ok(result);
    }

    public async Task<Result<ProductDTO>> CreateAsync(ProductCreateDTO dto)
    {
        var product = _mapper.Map<Product>(dto);
        await _unitOfWork.Product.AddAsync(product);
        await _unitOfWork.SaveAsync();

        var result = _mapper.Map<ProductDTO>(product);
        return Result.Ok(result);
    }

    public async Task<Result<ProductDTO>> UpdateAsync(Guid id, ProductUpdateDTO dto)
    {
        var entity = await _unitOfWork.Product.GetAsync(x => x.Id == id);
        if (entity == null) return DefaultNotFound<ProductDTO>(id);

        _mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Product.Update(entity);
        await _unitOfWork.SaveAsync();

        var result = _mapper.Map<ProductDTO>(entity);
        return Result.Ok(result);
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var entity = await _unitOfWork.Product.GetAsync(x => x.Id == id);
        if (entity == null) return DefaultNotFound(id, nameof(Product));

        _unitOfWork.Product.Remove(entity);
        await _unitOfWork.SaveAsync();

        return Result.Ok();
    }

    public async Task<Result<FileUploadResponse>> GetSignedUrl(Guid id, string imageName)
    {
        var entity = await _unitOfWork.Product.GetAsync(x => x.Id == id);
        if (entity == null) return DefaultNotFound<FileUploadResponse>(id);

        try
        {
            return Result.Ok(await _blobService.GenerateSignedUriAsync("product-images", $"{id}/{imageName}"));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to generate signed URL").CausedBy(ex));
        }
    }
}
