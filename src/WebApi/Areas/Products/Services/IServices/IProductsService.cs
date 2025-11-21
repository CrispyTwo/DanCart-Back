using DanCart.DataAccess.Models;
using DanCart.DataAccess.Models.Utility;
using DanCart.Models.Products;
using DanCart.Products.Models.DTOs;
using DanCart.WebApi.Areas.Products.DTOs;
using FluentResults;

namespace DanCart.WebApi.Areas.Products.Services.IServices;

public interface IProductsService
{
    public Task<Result<IEnumerable<ProductDTO>>> GetAsync(Page page, ProductStockStatus? status, string? sort);
    public Task<Result<ProductWithImagesDTO>> GetByIdAsync(Guid id);

    public Task<Result<ProductDTO>> CreateAsync(ProductCreateDTO dto);
    public Task<Result<ProductDTO>> UpdateAsync(Guid id, ProductUpdateDTO dto);
    public Task<Result> DeleteAsync(Guid id);
    public Task<Result<FileUploadResponse>> GetSignedUrl(Guid id, string imageName);
}
