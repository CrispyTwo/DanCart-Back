using DanCart.WebApi.Areas.Products.DTOs;

namespace DanCart.WebApi.Areas.ShoppingCarts.DTOs;

public class CartItemDTO
{
    public required ProductDTO Product { get; set; }
    public int Quantity { get; set; }
    public decimal Total => Product.Price * Quantity;
}
