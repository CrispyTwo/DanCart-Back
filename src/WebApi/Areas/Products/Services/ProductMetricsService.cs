using DanCart.DataAccess.Repository.IRepository;
using DanCart.WebApi.Areas.Products.Services.IServices;
using DanCart.WebApi.Areas.Products.DTOs.Metrics;
using FluentResults;
using DanCart.Models.Products;

namespace DanCart.WebApi.Areas.Products.Services;

public class ProductMetricsService(IUnitOfWork _unitOfWork) : IProductMetricsService
{
    public async Task<Result<long>> GetProductMetricsAsync(ProductMetricsQuery query, CancellationToken ct = default)
    {
        long quantity = query.Status switch
        {
            ProductStockStatus.InStock => await _unitOfWork.Product.GetTotalAsync(x => x.Stock > x.LowStockThreshold),
            ProductStockStatus.LowStock => await _unitOfWork.Product.GetTotalAsync(x => x.Stock <= x.LowStockThreshold && x.Stock > 0),
            ProductStockStatus.OutOfStock => await _unitOfWork.Product.GetTotalAsync(x => x.Stock <= 0),
            _ => await _unitOfWork.Product.GetTotalAsync(),
        };

        return Result.Ok(quantity);
    }
}
