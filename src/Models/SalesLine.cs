using DanCart.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanCart.Models;

public class SalesLine
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid SalesOrderId { get; set; }
    [ForeignKey(nameof(SalesOrderId))]
    public SalesOrder SalesOrder { get; set; }

    [Required]
    public Guid ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }

    public int Count { get; set; }
    [Column(TypeName = SqlColumnTypes.Decimal10_2)]
    public decimal Price { get; set; }
}
