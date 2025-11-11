using AutoMapper;
using DanCart.DataAccess.Blob;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Products.Models.DTOs;
using DanCart.WebApi.Areas.Products.Services.IServices;
using DanCart.WebApi.Areas.Products.DTOs;
using FluentResults;
using DanCart.WebApi.Core;
using DanCart.DataAccess.Models;
using DanCart.Models.Products;
using DanCart.DataAccess.Models.Utility;

namespace DanCart.WebApi.Areas.Products.Services;

public class ProductsService(IUnitOfWork _unitOfWork, IMapper _mapper, IBlobService _blobService) : ServiceBase, IProductsService
{
    public async Task<Result<IEnumerable<ProductDTO>>> GetAsync(int page, int pageSize)
    {
        const int MinPageSize = 10, MaxPageSize = 50;
        pageSize = GetPageSize(pageSize, MinPageSize, MaxPageSize);

        var products = await _unitOfWork.Product.GetRangeAsync(new Page { Number = page, Size = pageSize });
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
