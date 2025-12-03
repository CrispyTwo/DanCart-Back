using DanCart.WebApi.Areas.SalesOrders.SalesLines.DTOs;

namespace DanCart.WebApi.Areas.SalesOrders.DTOs;

public class SalesOrderCreateDTO
{
    //public SalesLineCreateDTO[] Lines { get; set; }

    public string Phone { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Region { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}
