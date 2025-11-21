using DanCart.Models.SalesOrders;

namespace DanCart.WebApi.Areas.SalesOrders.DTOs;

public class SalesOrderWithLinesDTO
{
    public required Guid Id { get; set; }
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public string? Name { get; set; }
    public decimal Total { get; set; }
    public required IEnumerable<SalesLineDTO> Lines { get; set; }
    public DateTime OrderDate { get; set; }
    public SalesOrderStatus OrderStatus { get; set; }
}
