namespace DanCart.WebApi.Areas.SalesOrders.SalesLines.DTOs;

public class SalesLineCreateDTO
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
