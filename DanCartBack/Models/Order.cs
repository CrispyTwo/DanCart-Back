using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAdmin.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string OrderNumber { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ShippingAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "pending"; // pending, processing, shipped, fulfilled, cancelled

        [MaxLength(50)]
        public string PaymentStatus { get; set; } = "pending"; // pending, paid, failed, refunded

        [MaxLength(100)]
        public string PaymentMethod { get; set; } = string.Empty;

        [MaxLength(200)]
        public string PaymentTransactionId { get; set; } = string.Empty;

        // Shipping information
        [MaxLength(200)]
        public string ShippingFirstName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string ShippingLastName { get; set; } = string.Empty;

        [MaxLength(300)]
        public string ShippingAddress1 { get; set; } = string.Empty;

        [MaxLength(300)]
        public string ShippingAddress2 { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ShippingCity { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ShippingState { get; set; } = string.Empty;

        [MaxLength(20)]
        public string ShippingZip { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ShippingCountry { get; set; } = string.Empty;

        [MaxLength(100)]
        public string TrackingNumber { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        // Foreign keys
        public int StoreId { get; set; }
        public int CustomerId { get; set; }

        // Navigation properties
        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; } = null!;

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } = null!;

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        // Foreign keys
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        // Navigation properties
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
    }
}
