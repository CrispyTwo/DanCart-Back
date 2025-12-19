namespace DanCart.WebApi.Areas.ShoppingCarts.DTOs;

public class ShoppingCartDTO
{
    public required IEnumerable<CartItemDTO> Items { get; set; }
    public decimal Total => Items.Sum(i => i.Total);
}
