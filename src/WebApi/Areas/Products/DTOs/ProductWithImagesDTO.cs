using DanCart.Models.Products;

namespace DanCart.WebApi.Areas.Products.DTOs;

public class ProductWithImagesDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Colors { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int LowStockThreshold { get; set; }
    public bool IsActive { get; set; }
    public decimal? Weight { get; set; }
    public UnitOfMeasure? WeightUnit { get; set; }
    public IEnumerable<string>? Urls { get; set; }
}