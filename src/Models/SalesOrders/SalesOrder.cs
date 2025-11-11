using DanCart.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanCart.Models.SalesOrders;

public class SalesOrder
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime ShippingDate { get; set; }

    public SalesOrderStatus OrderStatus { get; set; } = SalesOrderStatus.Created;
    public SalesOrderPaymentStatus PaymentStatus { get; set; } = SalesOrderPaymentStatus.Pending;
    public string? TrackingNumber { get; set; }
    public string? Carrier { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime PaymentDueDate { get; set; }
    public string? SessionId { get; set; }
    public string? PaymentIntendId { get; set; }

    public ICollection<SalesLine> SalesLines { get; set; } = [];

    [Required]
    public string Phone { get; set; }
    [Required]
    public string Street { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public string Region { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Name { get; set; }
}
