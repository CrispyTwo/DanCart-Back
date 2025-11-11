using DanCart.WebApi.Areas.SalesOrders.Services.IServices;
using DanCart.WebApi.Areas.SalesOrders.DTOs.Metrics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DanCart.WebApi.Core;
using DanCart.Models.SalesOrders;
using DanCart.Models.Auth;

namespace DanCart.WebApi.Areas.SalesOrders.Controllers;

[ApiController, Route("api/v1/admin/metrics/orders"), Authorize(Roles = UserRole.Admin)]
public class SalesOrderMetricsController(ISalesOrderMetricsService _metricsService) : APIControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSalesOrderMetricsAsync([FromQuery] SalesOrderMetric metric,
        [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null, 
        [FromQuery] SalesOrderStatus? status = null)
    {
        SalesOrderMetricsQuery query = new() { Metric = metric, From = from, To = to, Status = status };

        var result = await _metricsService.GetSalesOrderMetricsAsync(query);
        return CreateHttpResponse(result);
    }
}
