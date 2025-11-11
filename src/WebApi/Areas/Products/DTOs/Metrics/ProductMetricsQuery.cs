using DanCart.Models.Products;

namespace DanCart.WebApi.Areas.Products.DTOs.Metrics;

public record ProductMetricsQuery
{
    public ProductStockStatus? Status { get; init; }
}

