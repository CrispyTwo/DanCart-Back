using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAdmin.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastOrderAt { get; set; }

        // Foreign keys
        public int StoreId { get; set; }

        // Navigation properties
        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // Computed properties
        [NotMapped]
        public int TotalOrders => Orders.Count;

        [NotMapped]
        public decimal TotalSpent => Orders.Where(o => o.Status == "fulfilled").Sum(o => o.Total);

        [NotMapped]
        public decimal AverageOrderValue => TotalOrders > 0 ? TotalSpent / TotalOrders : 0;

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
