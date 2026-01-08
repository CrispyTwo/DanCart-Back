using DanCart.Models.Products;
using DanCart.WebApi.Areas.Products.DTOs;
using FluentResults;
using static DanCart.WebApi.Areas.Inventory.Services.InventoryService;

namespace DanCart.WebApi.Areas.Inventory.Services.IServices;

public interface IInventoryService
{
    public Task<Result> UpdateAsync(ProductDTO product, ProductVariant dims, (int quantity, InventoryOperation operation)? val = null);
    public Task<Result> DeleteAsync(ProductDTO product, ProductVariant dims);
}
