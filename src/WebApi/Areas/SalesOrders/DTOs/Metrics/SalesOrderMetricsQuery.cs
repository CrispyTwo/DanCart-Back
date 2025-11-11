using DanCart.Models.SalesOrders;

namespace DanCart.WebApi.Areas.SalesOrders.DTOs.Metrics;

public class SalesOrderMetricsQuery
{
    public DateTimeOffset? From { get; init; }
    public DateTimeOffset? To { get; init; }

    public SalesOrderStatus? Status { get; init; }
    public SalesOrderMetric? Metric { get; init; }
}
