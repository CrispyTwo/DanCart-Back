using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAdmin.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Sku { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? CompareAtPrice { get; set; }

        public int Stock { get; set; }
        public int LowStockThreshold { get; set; } = 5;

        [MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public bool TrackQuantity { get; set; } = true;

        [Column(TypeName = "decimal(8,2)")]
        public decimal Weight { get; set; }

        [MaxLength(20)]
        public string WeightUnit { get; set; } = "lb";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public int StoreId { get; set; }

        // Navigation properties
        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // Computed properties
        public string StockStatus => Stock <= 0 ? "out" : Stock <= LowStockThreshold ? "low" : "active";
    }
}
