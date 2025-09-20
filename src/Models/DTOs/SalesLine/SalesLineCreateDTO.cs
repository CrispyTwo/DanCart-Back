namespace DanCart.Models.DTOs.SalesLine;

public class SalesLineCreateDTO
{
    public Guid ProductId { get; set; }
    public int Count { get; set; }
}
