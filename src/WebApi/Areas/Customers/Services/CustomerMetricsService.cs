using DanCart.DataAccess.Repository.IRepository;
using DanCart.WebApi.Areas.Customers.DTOs.Metrics;
using DanCart.WebApi.Areas.Customers.Services.IServices;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace DanCart.WebApi.Areas.Customers.Services;

public class CustomerMetricsService(IUnitOfWork _unitOfWork) : ICustomerMetricsService
{
    public async Task<Result<long>> GetCustomerMetricsAsync(CustomerMetricsQuery query, CancellationToken ct = default)
    {
        var q = _unitOfWork.ApplicationUser.GetQuery();

        if (query.From.HasValue)
            q = q.Where(c => c.CreatedAt >= query.From.Value.UtcDateTime);

        if (query.To.HasValue)
            q = q.Where(c => c.CreatedAt <= query.To.Value.UtcDateTime);

        if (query.IsActive.HasValue)
            q = q.Where(c => c.IsActive == query.IsActive.Value);

        return Result.Ok(await q.LongCountAsync(ct));
    }
}