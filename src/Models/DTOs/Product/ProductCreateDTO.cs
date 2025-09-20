using Microsoft.AspNetCore.Http;
using DanCart.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanCart.Models.DTOs.Product;

public class ProductCreateDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int LowStockThreshold { get; set; }
    public decimal Weight { get; set; }
    public string? WeightUnit { get; set; }

    public IFormFile? Image { get; set; }
}
