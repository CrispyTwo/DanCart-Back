using DanCart.WebApi.Areas.SalesOrders.Services.IServices;
using DanCart.WebApi.Areas.SalesOrders.DTOs;
using DanCart.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DanCart.WebApi.Core;
using DanCart.Models.Auth;

namespace DanCart.WebApi.Areas.SalesOrders.Controllers;

[ApiController, Route("api/v1/[controller]"), Authorize]
public class SalesOrdersController(ISalesOrdersService _orderService) : APIControllerBase
{
    #region CRUD APIs
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 1)
    {
        var result = await _orderService.GetAsync(User.GetUserId(), page, pageSize, User.IsAdmin());
        return CreateHttpResponse(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _orderService.GetAsync(User.GetUserId(), id, User.IsAdmin());
        return CreateHttpResponse(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SalesOrderCreateDTO model)
    {
        var result = await _orderService.CreateAsync(User.GetUserId(), model);
        return CreateHttpResponse(result, 201);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _orderService.DeleteAsync(User.GetUserId(), id, User.IsAdmin());
        return CreateHttpResponse<SalesOrderWithLinesDTO>(result, 204);
    }
    #endregion
}