
using DanCart.WebApi.Areas.Products.Services.IServices;
using DanCart.WebApi.Areas.Products.DTOs.Metrics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DanCart.WebApi.Core;
using DanCart.Models.Products;
using DanCart.Models.Auth;

namespace DanCart.WebApi.Areas.Products.Controllers;

[ApiController, Route("api/v1/admin/metrics/products"), Authorize(Roles = UserRole.Admin)]
public class ProductMetricsController(IProductMetricsService _metricsService) : APIControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] ProductStockStatus? productStatus = null)
    {
        ProductMetricsQuery query = new() { Status = productStatus };

        var result = await _metricsService.GetProductMetricsAsync(query);
        return CreateHttpResponse(result);
    }
}
