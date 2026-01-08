using DanCart.DataAccess.Models;
using DanCart.DataAccess.Models.Utility;
using DanCart.Models.Products;
using DanCart.Products.Models.DTOs;
using DanCart.WebApi.Areas.Products.DTOs;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;

namespace DanCart.WebApi.Areas.Products.Services.IServices;

public interface IProductsService
{
    public Task<Result<IEnumerable<ProductDTO>>> GetAsync(Page page, ProductStockStatus? status, string? priceRange, string[]? categories, string? sort, string? search, bool? inStock);
    public Task<Result<IEnumerable<ProductDTO>>> GetByIdAsync(IEnumerable<Guid> ids);
    public Task<Result<ProductDTO>> GetByIdAsync(Guid id);

    public Task<Result<ProductDTO>> CreateAsync(ProductCreateDTO dto);
    public Task<Result<ProductDTO>> UpdateAsync(Guid id, ProductUpdateDTO dto);
    public Task<Result<ProductDTO>> UpdateAsync(Guid id, JsonPatchDocument<ProductUpdateDTO> patchDoc);
    public Task<Result> DeleteAsync(Guid id);
}
