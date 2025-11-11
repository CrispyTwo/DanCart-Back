using DanCart.WebApi.Areas.Products.DTOs.Metrics;
using FluentResults;

namespace DanCart.WebApi.Areas.Products.Services.IServices;

public interface IProductMetricsService
{
    Task<Result<long>> GetProductMetricsAsync(ProductMetricsQuery query, CancellationToken ct = default);
}
