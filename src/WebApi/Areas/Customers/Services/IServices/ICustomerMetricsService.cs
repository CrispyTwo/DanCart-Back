using DanCart.WebApi.Areas.Customers.DTOs.Metrics;
using FluentResults;

namespace DanCart.WebApi.Areas.Customers.Services.IServices;

public interface ICustomerMetricsService
{
    Task<Result<long>> GetCustomerMetricsAsync(CustomerMetricsQuery query, CancellationToken ct = default);
}
