using DanCart.Models.SalesOrders;
using DanCart.Models.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;

namespace DanCart.Models.Products;

public class Product : IFullTextSearchable
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(200)]
    public required string Name { get; set; }

    [Required, MaxLength(2000)]
    public required string Description { get; set; }
    [Required, MaxLength(50)]
    public required string Category { get; set; }
    public string? Colors { get; set; }
    public ICollection<SalesLine> SalesLines { get; set; } = [];


    [Column(TypeName = SqlColumnTypes.Decimal10_2)]
    public decimal Price { get; set; }

    public int Stock { get; set; }
    public int LowStockThreshold { get; set; } = 5;

    public bool IsActive { get; set; } = true;


    [Column(TypeName = SqlColumnTypes.Decimal10_2)]
    public decimal Weight { get; set; } = 1m;

    [MaxLength(5)]
    public UnitOfMeasure WeightUnit { get; set; } = UnitOfMeasure.Kg;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public NpgsqlTsVector SearchVector { get; set; } = default!;
}
