using DanCart.Models.DTOs.Product;
using DanCart.Utility;
using DanCart.WebApi.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanCart.WebApi.Controllers;

[ApiController, Route("api/v1/[controller]"), Authorize(Roles = UserRole.Admin)]
public class ProductsController(IProductService _productService) : APIControllerBase
{
    #region CRUD APIs
    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 1)
    {
        var result = await _productService.GetAsync(page, pageSize);
        if (result.IsSuccess) return Ok(result.Value);

        return BadRequest(result.Errors);
    }

    [HttpGet("{id}"), AllowAnonymous]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _productService.GetByIdAsync(id);
        if (result.IsSuccess) return Ok(result.Value);

        var handledError = ProcessGenericError(result);
        if (handledError != null) return handledError;

        return BadRequest(result.Errors[0].Message);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDTO model)
    {
        var result = await _productService.CreateAsync(model);
        if (result.IsSuccess) return CreatedAtAction(nameof(Create), result.Value);

        return BadRequest(result.Errors);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDTO model)
    {
        var result = await _productService.UpdateAsync(id, model);
        if (result.IsSuccess) return Ok(result.Value);

        var handledError = ProcessGenericError(result);
        if (handledError != null) return handledError;

        return BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var result = await _productService.DeleteAsync(id);
        if (result.IsSuccess) return NoContent();

        var handledError = ProcessGenericError(result);
        if (handledError != null) return handledError;

        return BadRequest(result.Errors);
    }
    #endregion 

    [HttpPut("{id}/images")]
    public async Task<IActionResult> UploadImage(Guid id, [FromForm] ProductFileUploadDTO model)
    {
        var result = await _productService.GetSignedUrl(id, model);
        if (result.IsSuccess) return Ok(result.Value);

        var handledError = ProcessGenericError(result);
        if (handledError != null) return handledError;

        return BadRequest(result.Errors);
    }
}
