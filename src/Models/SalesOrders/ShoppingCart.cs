using DanCart.Models.Auth;
using DanCart.Models.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanCart.Models.SalesOrders;

public class ShoppingCart
{
    public required string UserId { get; set; }
    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }
    public required Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
    [Range(1, 100, ErrorMessage = "Please enter value between 1 - 100")]
    public required int Quantity { get; set; }
    [NotMapped]
    public decimal UnitPrice { get; set; }
}
