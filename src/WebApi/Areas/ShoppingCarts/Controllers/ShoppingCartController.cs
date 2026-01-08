using DanCart.DataAccess.Models.Utility;
using DanCart.WebApi.Areas.Products.DTOs;
using DanCart.WebApi.Areas.Products.Services.IServices;
using DanCart.WebApi.Areas.ShoppingCarts.DTOs;
using DanCart.WebApi.Areas.ShoppingCarts.Services.IServices;
using DanCart.WebApi.Core;
using DanCart.WebApi.Extensions;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DanCart.WebApi.Areas.ShoppingCarts.Controllers;

[ApiController, Route("api/v1/[controller]"), Authorize]
public class CartController(IShoppingCartService _shoppingCartService) : APIControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 1, [FromQuery] string? sort = null)
    {
        var result = await _shoppingCartService.GetAsync(User.GetUserId(), new Page(page, pageSize), sort);
        return CreateHttpResponse(result);
    }

    public sealed record AddRequest(Guid ProductId, ProductVariant Variant, int? Quantity);
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddRequest request)
    {
        Result result;
        if (request.Quantity == null)
            result = await _shoppingCartService.AddAsync(User.GetUserId(), request.ProductId, request.Variant);
        else
            result = await _shoppingCartService.AddAsync(User.GetUserId(), request.ProductId, request.Variant, (int)request.Quantity);

        return CreateHttpResponse<ShoppingCartDTO>(result, 201);
    }


    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> Delete(Guid productId, [FromBody] ProductVariant variant)
    {
        var result = await _shoppingCartService.DeleteAsync(User.GetUserId(), productId, variant);
        return CreateHttpResponse<ShoppingCartDTO>(result, 204);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        var result = await _shoppingCartService.DeleteAsync(User.GetUserId());
        return CreateHttpResponse<ShoppingCartDTO>(result, 204);
    }
}
