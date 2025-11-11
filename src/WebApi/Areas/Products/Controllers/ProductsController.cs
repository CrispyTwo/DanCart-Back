using DanCart.Products.Models.DTOs;
using DanCart.WebApi.Areas.Products.Services.IServices;
using DanCart.WebApi.Areas.Products.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DanCart.WebApi.Core;
using DanCart.Models.Auth;

namespace DanCart.WebApi.Areas.Products.Controllers;

[ApiController, Route("api/v1/[controller]"), Authorize(Roles = UserRole.Admin)]
public class ProductsController(IProductsService _productService) : APIControllerBase
{
    #region CRUD APIs
    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 1)
    {
        var result = await _productService.GetAsync(page, pageSize);
        return CreateHttpResponse(result);
    }

    [HttpGet("{id}"), AllowAnonymous]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _productService.GetByIdAsync(id);
        return CreateHttpResponse(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDTO model)
    {
        var result = await _productService.CreateAsync(model);
        return CreateHttpResponse(result, 201);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDTO model)
    {
        var result = await _productService.UpdateAsync(id, model);
        return CreateHttpResponse(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _productService.DeleteAsync(id);
        return CreateHttpResponse<ProductDTO>(result, 204);
    }
    #endregion

    [HttpPut("{id}/images/{imageName}")]
    public async Task<IActionResult> UploadImage(Guid id, string imageName)
    {
        var result = await _productService.GetSignedUrl(id, imageName);
        return CreateHttpResponse(result);
    }
}
