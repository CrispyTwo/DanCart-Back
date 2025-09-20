using DanCart.Models.DTOs.SalesLine;
using System.ComponentModel.DataAnnotations;

namespace DanCart.Models.DTOs.SalesOrder;

public class SalesOrderCreateDTO
{
    public SalesLineCreateDTO[] Lines { get; set; }

    public string Phone { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Region { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}
