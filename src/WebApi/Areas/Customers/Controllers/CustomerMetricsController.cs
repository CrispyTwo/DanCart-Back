using DanCart.Models.Auth;
using DanCart.WebApi.Areas.Customers.DTOs.Metrics;
using DanCart.WebApi.Areas.Customers.Services.IServices;
using DanCart.WebApi.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanCart.WebApi.Areas.Customers.Controllers;

[ApiController, Route("api/v1/admin/metrics/customers"), Authorize(Roles = UserRole.Admin)]
public class CustomerMetricsController(ICustomerMetricsService _metricsService) : APIControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] DateTime? From = null, [FromQuery] DateTime? To = null, [FromQuery] bool? IsActive = null)
    {
        CustomerMetricsQuery query = new() { From = From, To = To, IsActive = IsActive };
        var result = await _metricsService.GetCustomerMetricsAsync(query);
        return CreateHttpResponse(result);
    }
}
