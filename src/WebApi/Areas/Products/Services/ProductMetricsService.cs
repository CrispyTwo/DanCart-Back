using DanCart.DataAccess.Repository.IRepository;
using DanCart.WebApi.Areas.Products.Services.IServices;
using DanCart.WebApi.Areas.Products.DTOs.Metrics;
using FluentResults;
using DanCart.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace DanCart.WebApi.Areas.Products.Services;

public class ProductMetricsService(IUnitOfWork _unitOfWork) : IProductMetricsService
{
    public async Task<Result<long>> GetProductMetricsAsync(ProductMetricsQuery query, CancellationToken ct = default)
    {
        long quantity = query.Status switch
        {
            ProductStockStatus.InStock => await _unitOfWork.Product.GetTotalAsync(x => x.Inventory.Sum(x => x.Quantity) > x.LowStockThreshold, "Inventory"),
            ProductStockStatus.LowStock => await _unitOfWork.Product.GetTotalAsync(x => x.Inventory.Sum(x => x.Quantity) <= x.LowStockThreshold && x.Inventory.Sum(x => x.Quantity) > 0, "Inventory"),
            ProductStockStatus.OutOfStock => await _unitOfWork.Product.GetTotalAsync(x => x.Inventory.Sum(x => x.Quantity) <= 0, "Inventory"),
            _ => await _unitOfWork.Product.GetTotalAsync(includeProperties: "Inventory"),
        };

        return Result.Ok(quantity);
    }
}
