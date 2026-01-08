using DanCart.Models.Products;
using DanCart.WebApi.Areas.Products.DTOs;
using System.Text.Json.Serialization;

namespace DanCart.WebApi.Areas.ShoppingCarts.DTOs;

public class CartItemDTO
{
    public required ProductDTO Product { get; set; }
    public required string Color { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProductSize Size { get; set; }
    public int Quantity { get; set; }
    public decimal Total => Product.Price * Quantity;
}
