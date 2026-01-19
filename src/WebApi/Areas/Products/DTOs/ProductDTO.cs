using DanCart.Models.Products;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DanCart.WebApi.Areas.Products.DTOs;

public class ProductDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int LowStockThreshold { get; set; }
    public bool IsActive { get; set; }
    public decimal? Weight { get; set; }
    public UnitOfMeasure? WeightUnit { get; set; }
    public IEnumerable<OptionGroupDto> Options { get; set; } = [];
    public IEnumerable<ProductVariant> Variants { get; set; } = [];
    public IEnumerable<string> Images { get; set; } = [];
}

public class OptionGroupDto
{
    public string Name { get; set; } = string.Empty;
    public IEnumerable<string> Values { get; set; } = [];
}

public class ProductVariant
{
    public string Color { get; set; } = string.Empty;
    [JsonConverter(typeof(StringEnumConverter))]
    public ProductSize Size { get; set; }
    public int Stock { get; set; }
}