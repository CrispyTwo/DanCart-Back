using DanCart.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanCart.Models.SalesOrders;

public class SalesOrder
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public required string UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser? User { get; set; }

    public ICollection<SalesLine> SalesLines { get; set; } = [];

    public SalesOrderStatus OrderStatus { get; set; } = SalesOrderStatus.Created;
    public SalesOrderPaymentStatus PaymentStatus { get; set; } = SalesOrderPaymentStatus.Pending;
    public DateTime ShippingDate { get; set; }
    public string? TrackingNumber { get; set; }
    public string? Carrier { get; set; }

    #region Payment
    public DateTime PaymentDate { get; set; }
    public DateTime PaymentDueDate { get; set; }
    public string? SessionId { get; set; }
    public string? PaymentIntendId { get; set; }
    #endregion

    #region Delivery
    [Required]
    public required string Phone { get; set; }
    [Required]
    public required string Street { get; set; }
    [Required]
    public required string City { get; set; }
    [Required]
    public required string Country { get; set; }
    [Required]
    public required string Region { get; set; }
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Name { get; set; }
    #endregion

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
}
