using DanCart.Models;
using DanCart.Models.DTOs.Product;
using FluentResults;

namespace DanCart.WebApi.Services.IServices;

public interface IProductService
{
    public Task<Result<IEnumerable<Product>>> GetAsync(int page, int pageSize);
    public Task<Result<Product>> GetByIdAsync(Guid id);

    public Task<Result<Product>> CreateAsync(ProductCreateDTO dto);
    public Task<Result<Product>> UpdateAsync(Guid id, ProductUpdateDTO dto);
    public Task<Result<Product>> DeleteAsync(Guid id);
    public Task<Result<ProductFileUploadResponse>> GetSignedUrl(Guid id, ProductFileUploadDTO dto);
}
