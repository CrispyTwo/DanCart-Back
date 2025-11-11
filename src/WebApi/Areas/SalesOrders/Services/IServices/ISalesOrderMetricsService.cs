using DanCart.WebApi.Areas.SalesOrders.DTOs.Metrics;
using FluentResults;

namespace DanCart.WebApi.Areas.SalesOrders.Services.IServices;

public interface ISalesOrderMetricsService
{
    Task<Result<decimal>> GetSalesOrderMetricsAsync(SalesOrderMetricsQuery query, CancellationToken ct = default);
}
