using DanCart.WebApi.Areas.SalesOrders.Services.IServices;
using DanCart.WebApi.Areas.SalesOrders.DTOs;
using DanCart.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DanCart.WebApi.Core;
using DanCart.Models.Auth;
using DanCart.DataAccess.Models.Utility;

namespace DanCart.WebApi.Areas.SalesOrders.Controllers;

[ApiController, Route("api/v1/[controller]"), Authorize]
public class SalesOrdersController(ISalesOrdersService _orderService) : APIControllerBase
{
    #region CRUD APIs
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 1, [FromQuery] string? sort = null)
    {
        var result = await _orderService.GetAsync(User.GetUserId(), new Page(page, pageSize), sort, User.IsAdmin());
        return CreateHttpResponse(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _orderService.GetAsync(User.GetUserId(), id, User.IsAdmin());
        return CreateHttpResponse(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _orderService.DeleteAsync(User.GetUserId(), id, User.IsAdmin());
        return CreateHttpResponse<SalesOrderWithLinesDTO>(result, 204);
    }
    #endregion
}