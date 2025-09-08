namespace DanCart.Models.DTOs.Product;

public class ProductUpdateDTO
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int LowStockThreshold { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }
    public decimal Weight { get; set; }
    public string? WeightUnit { get; set; }
}
