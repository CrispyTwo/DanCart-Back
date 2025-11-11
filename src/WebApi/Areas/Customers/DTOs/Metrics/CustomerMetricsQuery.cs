namespace DanCart.WebApi.Areas.Customers.DTOs.Metrics;

public class CustomerMetricsQuery
{
    public DateTimeOffset? From { get; init; }
    public DateTimeOffset? To { get; init; }

    public bool? IsActive { get; init; }
}