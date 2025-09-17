using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanCart.Models;

public class SalesOrder
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public DateTime ShippingDate { get; set; }

    public string? OrderStatus { get; set; }
    public string? PaymentStatus { get; set; }
    public string? TrackingNumber { get; set; }
    public string? Carrier { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime PaymentDueDate { get; set; }
    public string? SessionId { get; set; }
    public string? PaymentIntendId { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string Street { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public string Region { get; set; }
    [Required]
    public string Name { get; set; }
}
