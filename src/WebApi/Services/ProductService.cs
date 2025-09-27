using AutoMapper;
using DanCart.DataAccess.Blob;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models;
using DanCart.Models.DTOs.Product;
using DanCart.WebApi.Controllers;
using DanCart.WebApi.Services.IServices;
using FluentResults;

namespace DanCart.WebApi.Services;

public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper, IBlobService _blobService) : ServiceBase, IProductService
{
    public async Task<Result<IEnumerable<Product>>> GetAsync(int page, int pageSize)
    {
        const int MinPageSize = 20, MaxPageSize = 50;
        pageSize = GetPageSize(pageSize, MinPageSize, MaxPageSize);

        return Result.Ok(await _unitOfWork.Product.GetRangeAsync(page, pageSize));
    }

    public async Task<Result<Product>> GetByIdAsync(Guid id)
    {
        var product = await _unitOfWork.Product.GetAsync(x => x.Id == id);
        if (product == null) return DefaultNotFound<Product>(id);

        return Result.Ok(product);
    }

    public async Task<Result<Product>> CreateAsync(ProductCreateDTO dto)
    {
        var product = _mapper.Map<Product>(dto);
        await _unitOfWork.Product.AddAsync(product);
        await _unitOfWork.SaveAsync();
        return Result.Ok(product);
    }

    public async Task<Result<Product>> UpdateAsync(Guid id, ProductUpdateDTO dto)
    {
        var entity = await _unitOfWork.Product.GetAsync(x => x.Id == id);
        if (entity == null) return DefaultNotFound<Product>(id);

        _mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Product.Update(entity);
        await _unitOfWork.SaveAsync();
        return Result.Ok(entity);
    }

    public async Task<Result<Product>> DeleteAsync(Guid id)
    {
        var entity = await _unitOfWork.Product.GetAsync(x => x.Id == id);
        if (entity == null) DefaultNotFound<Product>(id);

        _unitOfWork.Product.Remove(entity);
        await _unitOfWork.SaveAsync();
        return Result.Ok(entity);
    }

    public async Task<Result<ProductFileUploadResponse>> GetSignedUrl(Guid id, ProductFileUploadDTO model)
    {
        var entity = await _unitOfWork.Product.GetAsync(x => x.Id == id);
        if (entity == null) DefaultNotFound<Product>(id);

        try
        {
            return Result.Ok(await _blobService.GenerateSignedUriAsync("Product-Images", model.FileName, ""));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to generate signed URL").CausedBy(ex));
        }
    }
}
