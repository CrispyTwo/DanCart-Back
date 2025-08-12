using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAdmin.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Domain { get; set; } = string.Empty;

        [MaxLength(500)]
        public string LogoUrl { get; set; } = string.Empty;

        [MaxLength(500)]
        public string HeroBannerUrl { get; set; } = string.Empty;

        [MaxLength(500)]
        public string FaviconUrl { get; set; } = string.Empty;

        // Design settings
        [MaxLength(7)]
        public string PrimaryColor { get; set; } = "#2563eb";

        [MaxLength(7)]
        public string SecondaryColor { get; set; } = "#4b5563";

        [MaxLength(7)]
        public string AccentColor { get; set; } = "#16a34a";

        [MaxLength(50)]
        public string FontFamily { get; set; } = "Inter";

        [MaxLength(50)]
        public string Template { get; set; } = "modern";

        // Business settings
        [MaxLength(3)]
        public string Currency { get; set; } = "USD";

        [MaxLength(100)]
        public string Timezone { get; set; } = "America/New_York";

        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxRate { get; set; } = 0;

        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        [Required]
        public string UserId { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public virtual StoreSettings Settings { get; set; } = null!;
    }
}
