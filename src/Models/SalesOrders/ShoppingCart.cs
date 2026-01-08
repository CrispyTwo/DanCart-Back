using DanCart.Models.Auth;
using DanCart.Models.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanCart.Models.SalesOrders;

public class ShoppingCart
{
    public required string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public required Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [Range(1, 100)]
    public required int Quantity { get; set; }
    public required string Color { get; set; }
    public required ProductSize Size { get; set; }
}

