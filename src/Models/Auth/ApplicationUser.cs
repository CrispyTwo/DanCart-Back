using DanCart.Models.SalesOrders;
using DanCart.Models.Utility;
using Microsoft.AspNetCore.Identity;
using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanCart.Models.Auth;

public class ApplicationUser : IdentityUser, IFullTextSearchable
{
    [Required, MaxLength(100), ProtectedPersonalData]
    public required string FirstName { get; set; }

    [Required, MaxLength(100), ProtectedPersonalData]
    public required string LastName { get; set; }

    public ICollection<SalesOrder> SalesOrders { get; set; } = [];

    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? HouseNumber { get; set; }

    [NotMapped]
    public string? Role { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    public NpgsqlTsVector SearchVector { get; set; } = default!;
}
