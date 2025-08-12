using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAdmin.Models
{
    public class StoreSettings
    {
        [Key]
        public int Id { get; set; }

        // Payment settings
        public bool StripeEnabled { get; set; } = false;
        [MaxLength(200)]
        public string StripePublishableKey { get; set; } = string.Empty;
        [MaxLength(200)]
        public string StripeSecretKey { get; set; } = string.Empty;

        public bool PayPalEnabled { get; set; } = false;
        [MaxLength(200)]
        public string PayPalClientId { get; set; } = string.Empty;
        [MaxLength(200)]
        public string PayPalClientSecret { get; set; } = string.Empty;

        public bool ApplePayEnabled { get; set; } = false;

        // Shipping settings
        public bool StandardShippingEnabled { get; set; } = true;
        [Column(TypeName = "decimal(8,2)")]
        public decimal StandardShippingRate { get; set; } = 5.99m;

        public bool ExpressShippingEnabled { get; set; } = false;
        [Column(TypeName = "decimal(8,2)")]
        public decimal ExpressShippingRate { get; set; } = 15.99m;

        public bool LocalPickupEnabled { get; set; } = false;

        // Email settings
        [MaxLength(200)]
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 587;
        [MaxLength(200)]
        public string SmtpUsername { get; set; } = string.Empty;
        [MaxLength(200)]
        public string SmtpPassword { get; set; } = string.Empty;
        public bool SmtpUseSsl { get; set; } = true;

        // Notification settings
        public bool EmailNotificationsEnabled { get; set; } = true;
        public bool OrderNotificationsEnabled { get; set; } = true;
        public bool LowStockNotificationsEnabled { get; set; } = true;

        // Foreign keys
        public int StoreId { get; set; }

        // Navigation properties
        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; } = null!;
    }
}
