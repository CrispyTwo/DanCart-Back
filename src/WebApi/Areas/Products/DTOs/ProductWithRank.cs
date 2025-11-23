using DanCart.Models.Products;

namespace DanCart.WebApi.Areas.Products.DTOs;

public class ProductWithRank
{
    public Product Product { get; set; } = null!;
    public float Rank { get; set; }
}
