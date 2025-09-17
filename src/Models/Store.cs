using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanCart.Models;

public class Store
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }

    public string Domain { get; set; }

    public string LogoUrl { get; set; }

    public string HeroBannerUrl { get; set; }

    public string FaviconUrl { get; set; }


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

    [MaxLength(500)]
    public string Address { get; set; }

    [MaxLength(20)]
    public string Phone { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
