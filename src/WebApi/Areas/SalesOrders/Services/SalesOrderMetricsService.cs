using DanCart.DataAccess.Repository.IRepository;
using DanCart.WebApi.Areas.SalesOrders.Services.IServices;
using DanCart.WebApi.Areas.SalesOrders.DTOs.Metrics;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using DanCart.Models.SalesOrders;

namespace DanCart.WebApi.Areas.SalesOrders.Services;

public class SalesOrderMetricsService(IUnitOfWork _unitOfWork) : ISalesOrderMetricsService
{
    public async Task<Result<decimal>> GetSalesOrderMetricsAsync(SalesOrderMetricsQuery query, CancellationToken ct = default)
    {
        IQueryable<SalesOrder> q = _unitOfWork.SalesOrder.GetQuery();

        if (query.From.HasValue)
        {
            var fromUtc = query.From.Value.UtcDateTime;
            q = q.Where(o => o.OrderDate >= fromUtc);
        }

        if (query.To.HasValue)
        {
            var toUtc = query.To.Value.UtcDateTime;
            q = q.Where(o => o.OrderDate <= toUtc);
        }

        if (query.Status.HasValue)
        {
            var status = query.Status;
            q = q.Where(o => o.OrderStatus == status);
        }

        switch (query.Metric)
        {
            case SalesOrderMetric.OrderCount:
                {
                    var count = await q.LongCountAsync(ct);
                    return Result.Ok((decimal)count);
                }

            case SalesOrderMetric.AverageOrderValue:
                {
                    var orderIds = q.Select(o => o.Id);

                    var aggregated = await _unitOfWork.SalesLine.GetQuery()
                        .Where(l => orderIds.Contains(l.SalesOrderId))
                        .GroupBy(l => l.SalesOrderId)
                        .Select(g => new
                        {
                            OrderTotal = g.Sum(l => (decimal?)(l.Quantity * l.Price)) ?? 0m
                        })
                        .GroupBy(x => 1)
                        .Select(g => new
                        {
                            Count = g.Count(),
                            Total = g.Sum(x => x.OrderTotal)
                        })
                        .FirstOrDefaultAsync(ct);

                    if (aggregated == null || aggregated.Count == 0)
                        return Result.Ok(0m);

                    var avg = aggregated.Total / aggregated.Count;
                    return Result.Ok(decimal.Round(avg, 2));
                }


            default:
                return Result.Fail<decimal>($"Unsupported metric: {query.Metric}");
        }
    }
}
