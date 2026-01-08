using AutoMapper;
using DanCart.DataAccess.Extensions;
using DanCart.DataAccess.Models.Utility;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models.Products;
using DanCart.Products.Models.DTOs;
using DanCart.WebApi.Areas.Products.DTOs;
using DanCart.WebApi.Areas.Products.Services.IServices;
using DanCart.WebApi.Core;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace DanCart.WebApi.Areas.Products.Services;

public class ProductsService(IUnitOfWork _unitOfWork, IMapper _mapper) : ServiceBase, IProductsService
{
    public async Task<Result<IEnumerable<ProductDTO>>> GetAsync(Page page, ProductStockStatus? status, string? priceRange, string[]? categories, string? sort, string? search, bool? inStock)
    {
        const int MinPageSize = 10, MaxPageSize = 50;
        page.ApplySizeRule(MinPageSize, MaxPageSize);

        var query = _unitOfWork.Product.GetQuery();
        query = query.Include(x => x.Inventory);
        switch (status)
        {
            case ProductStockStatus.InStock:
                query = query.Where(x => x.Inventory.Sum(x => x.Quantity) > x.LowStockThreshold);
                break;
            case ProductStockStatus.LowStock:
                query = query.Where(x => x.Inventory.Sum(x => x.Quantity) <= x.LowStockThreshold && x.Inventory.Sum(x => x.Quantity) > 0);
                break;
            case ProductStockStatus.OutOfStock:
                query = query.Where(x => x.Inventory.Sum(x => x.Quantity) <= 0);
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

        foreach (var item in result)
        {
            item.Stock = item.Variants.Sum(v => v.Stock);
        }

        if (inStock != null && inStock == true)
        {
            result = result.Where(x => x.Stock > 0);
        }

        return Result.Ok(result);
    }

    public async Task<Result<IEnumerable<ProductDTO>>> GetByIdAsync(IEnumerable<Guid> ids)
    {
        var products = await _unitOfWork.Product.GetQuery().Where(x => ids.Contains(x.Id)).Include(x => x.Inventory).ToListAsync();
        var dtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

        foreach (var item in dtos)
        {
            item.Stock = item.Variants.Sum(v => v.Stock);
        }

        var orderedProducts = ids.Join(dtos, id => id, prod => prod.Id, (id, prod) => prod);
        return Result.Ok(orderedProducts);
    }

    public async Task<Result<ProductDTO>> GetByIdAsync(Guid id)
    {
        var product = await _unitOfWork.Product.GetAsync(x => x.Id == id, includeProperties: "Inventory");
        if (product == null) return DefaultNotFound<ProductDTO>(id);

        product.Inventory = [.. product.Inventory.Where(x => x.Quantity > 0)];
        var result = _mapper.Map<ProductDTO>(product);

        var max = result.Variants.Select(x => ((int)x.Size)).Max();
        var min = result.Variants.Select(x => ((int)x.Size)).Min();
        var sizes = new int[max - min + 1];
        for (int i = 0; i < sizes.Length; i++)
        {
            sizes[i] = i + min;
        }

        result.Options = [
            new() { Name = "Color", Values = result.Variants.Select(x => x.Color).Distinct().OrderBy(x => x) },
            new() { Name = "Size", Values = sizes.Select(x => ((ProductSize)x).ToString()) }
        ];

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

    public async Task<Result<ProductDTO>> UpdateAsync(Guid id, JsonPatchDocument<ProductUpdateDTO> patchDoc)
    {
        var entity = await _unitOfWork.Product.GetAsync(x => x.Id == id);
        if (entity == null) return DefaultNotFound<ProductDTO>(id);

        var dtoToPatch = _mapper.Map<ProductUpdateDTO>(entity);
        patchDoc.ApplyTo(dtoToPatch);
        _mapper.Map(dtoToPatch, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Product.Update(entity);
        await _unitOfWork.SaveAsync();

        var result = _mapper.Map<ProductDTO>(entity);
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
}
