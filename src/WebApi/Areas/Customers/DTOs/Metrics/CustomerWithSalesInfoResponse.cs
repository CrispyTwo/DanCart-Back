namespace DanCart.WebApi.Areas.Customers.DTOs.Metrics;

public class CustomerWithSalesInfoResponse
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public long OrdersCount { get; set; }
    public decimal TotalSpent { get; set; }
    public bool IsActive { get; set; }
}