using DanCart.Models.DTOs.SalesOrder;
using DanCart.WebApi.Extensions;
using DanCart.WebApi.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanCart.WebApi.Controllers;

[ApiController, Route("api/v1/[controller]"), Authorize]
public class OrdersController(IOrderService _orderService) : APIControllerBase
{
    #region CRUD APIs
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 1)
    {
        var result = await _orderService.GetAsync(User.GetUserId(), page, pageSize);
        if (result.IsSuccess) Ok(result.Value);

        return BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _orderService.GetAsync(User.GetUserId(), id);
        if (result.IsSuccess) return Ok(result.Value);

        var handledError = ProcessGenericError(result);
        if (handledError != null) return handledError;

        return BadRequest(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SalesOrderCreateDTO model)
    {
        var result = await _orderService.CreateAsync(User.GetUserId(), model);
        if (result.IsSuccess) return CreatedAtAction(nameof(Get), result.Value);

        return BadRequest(result.Errors);
    }
    #endregion
}