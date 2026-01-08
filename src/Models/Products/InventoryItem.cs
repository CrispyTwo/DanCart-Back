using System.ComponentModel.DataAnnotations.Schema;

namespace DanCart.Models.Products;

public class InventoryItem
{
    public Guid ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    public int Quantity { get; set; } = 0;
    public string Color { get; set; } = string.Empty;
    public ProductSize Size { get; set; }
}
