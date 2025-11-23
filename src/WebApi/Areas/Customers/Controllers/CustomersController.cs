
using DanCart.DataAccess.Models.Utility;
using DanCart.Models.Auth;
using DanCart.WebApi.Areas.Customers.Services.IServices;
using DanCart.WebApi.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanCart.WebApi.Areas.Customers.Controllers;

[ApiController, Route("api/v1/[controller]"), Authorize(Roles = UserRole.Admin)]
public class CustomersController(ICustomerService _customerService) : APIControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 1, 
        [FromQuery] bool? isActive = null, [FromQuery] string? sort = null,
        [FromQuery] string? search = null)
    {
        var result = await _customerService.GetCustomersWithSalesAsync(new Page(page, pageSize), isActive, sort, search);
        return CreateHttpResponse(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _customerService.GetByIdAsync(id);
        return CreateHttpResponse(result);
    }
}
