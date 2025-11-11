using DanCart.Models.Products;

namespace DanCart.Products.Models.DTOs;

public class ProductCreateDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int LowStockThreshold { get; set; }
    public decimal Weight { get; set; }
    public UnitOfMeasure? WeightUnit { get; set; }

}
