using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models.Products;
using DanCart.WebApi.Areas.Inventory.Services.IServices;
using DanCart.WebApi.Areas.Products.DTOs;
using DanCart.WebApi.Core;
using FluentResults;

namespace DanCart.WebApi.Areas.Inventory.Services;

public class InventoryService(IUnitOfWork _unitOfWork) : ServiceBase, IInventoryService
{
    public enum InventoryOperation { Increase, Decrease, Replace };
    public async Task<Result> UpdateAsync(ProductDTO product, ProductVariant dims, (int quantity, InventoryOperation operation)? val)
    {
        var inventory = await _unitOfWork.Inventory.GetAsync(x => x.ProductId == product.Id && x.Color == dims.Color && x.Size == dims.Size, tracked: true);

        if (inventory == null)
        {
            inventory = new()
            {
                ProductId = product.Id,
                Color = dims.Color,
                Size = dims.Size,
                Quantity = val.HasValue ? val.Value.quantity : 0
            };

            await _unitOfWork.Inventory.AddAsync(inventory);
        }
        else
        {
            if (!val.HasValue) return Result.Ok();

            var qty = val.Value.quantity;
            switch (val.Value.operation)
            {
                case InventoryOperation.Increase:
                    inventory.Quantity += qty;
                    break;
                case InventoryOperation.Decrease:
                    inventory.Quantity -= qty;
                    break;
                case InventoryOperation.Replace:
                    inventory.Quantity = qty;
                    break;
            }
        }

        await _unitOfWork.SaveAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(ProductDTO product, ProductVariant dims)
    {
        var inventory = await _unitOfWork.Inventory.GetAsync(x => x.ProductId == product.Id && x.Color == dims.Color && x.Size == dims.Size);
        if (inventory == null)
        {
            return Result.Fail(
                new Error($"{nameof(InventoryItem)} with specific dimensions: {dims} not found")
                    .WithMetadata(ErrorMetadata.Code, ErrorCode.NotFound));
        }

        _unitOfWork.Inventory.Remove(inventory);
        return Result.Ok();
    }
}
