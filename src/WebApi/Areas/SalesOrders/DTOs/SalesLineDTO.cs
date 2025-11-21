using DanCart.Models.Products;
using DanCart.Models.SalesOrders;
using DanCart.Models.Utility;
using DanCart.WebApi.Areas.Products.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanCart.WebApi.Areas.SalesOrders.DTOs;

public class SalesLineDTO
{
    public Guid Id { get; set; }
    public Guid SalesOrderId { get; set; }
    public required ProductDTO Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
