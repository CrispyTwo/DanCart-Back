using System.ComponentModel.DataAnnotations;

namespace ECommerceAdmin.DTOs
{
    public class StoreDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string HeroBannerUrl { get; set; } = string.Empty;
        public string FaviconUrl { get; set; } = string.Empty;
        public string PrimaryColor { get; set; } = string.Empty;
        public string SecondaryColor { get; set; } = string.Empty;
        public string AccentColor { get; set; } = string.Empty;
        public string FontFamily { get; set; } = string.Empty;
        public string Template { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string Timezone { get; set; } = string.Empty;
        public decimal TaxRate { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ProductCount { get; set; }
        public int OrderCount { get; set; }
        public int CustomerCount { get; set; }
    }

    public class CreateStoreDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateStoreDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

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

        [MaxLength(3)]
        public string Currency { get; set; } = "USD";

        [MaxLength(100)]
        public string Timezone { get; set; } = "America/New_York";

        public decimal TaxRate { get; set; }

        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;
    }

    public class StoreAnalyticsDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public decimal RecentRevenue { get; set; }
        public int RecentOrders { get; set; }
        public int ActiveProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
    }
}
