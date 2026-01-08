using DanCart.DataAccess.Models.Utility;
using DanCart.Models.Auth;
using DanCart.Models.Products;
using DanCart.Products.Models.DTOs;
using DanCart.WebApi.Areas.Inventory.Services.IServices;
using DanCart.WebApi.Areas.Products.DTOs;
using DanCart.WebApi.Areas.Products.Services.IServices;
using DanCart.WebApi.Core;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using static DanCart.WebApi.Areas.Inventory.Services.InventoryService;

namespace DanCart.WebApi.Areas.Products.Controllers;

[ApiController, Route("api/v1/[controller]"), Authorize(Roles = UserRole.Admin)]
public class ProductsController(IProductsService _productService, IProductsBlobService _productBlobService, 
    IProductsVectorSearchService _vectorSearchService, IInventoryService _inventoryService) : APIControllerBase
{
    #region CRUD APIs
    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> Get(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 1, 
        [FromQuery] ProductStockStatus? status = null,
        [FromQuery] string? priceRange = null,
        [FromQuery] string? categories = null,
        [FromQuery] string? sort = null,
        [FromQuery] string? search = null,
        [FromQuery] string? aiSearch = null,
        [FromQuery] bool? inStock = false)
    {
        Result<IEnumerable<ProductDTO>> result;
        if (!string.IsNullOrWhiteSpace(aiSearch))
        {
            var ranking = await _vectorSearchService.GetRankingAsync(aiSearch);

            if (ranking.IsFailed)
            {
                return CreateHttpResponse(ranking);
            }
            else
            {
                result = await _productService.GetByIdAsync(ranking.Value.Select(x => Guid.Parse(x.ProductId)));
            }
        }
        else
        {
            result = await _productService.GetAsync(new Page(page, pageSize), status, priceRange, categories?.Split(','), sort, search, inStock);
        }

        if (result.IsSuccess)
        {
            result = result.Map(products => _productBlobService.AttachImages(products));
        }

        return CreateHttpResponse(result);
    }

    [HttpGet("{id}"), AllowAnonymous]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _productService.GetByIdAsync(id);
        if (result.IsSuccess)
        {
            result.Map(product => _productBlobService.AttachImages(product));
        }

        return CreateHttpResponse(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDTO model)
    {
        var result = await _productService.CreateAsync(model);
        if (result.IsSuccess)
        {
            await _vectorSearchService.MergeOrCreateAsync(result.Value.Id);
        }

        return CreateHttpResponse(result, 201);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<ProductUpdateDTO> model)
    {
        var result = await _productService.UpdateAsync(id, model);
        if (result.IsSuccess)
        {
            await _vectorSearchService.MergeOrCreateAsync(result.Value.Id);
        }

        return CreateHttpResponse(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] ProductUpdateDTO model)
    {
        var result = await _productService.UpdateAsync(id, model);
        if (result.IsSuccess)
        {
            await _vectorSearchService.MergeOrCreateAsync(result.Value.Id);
        }

        return CreateHttpResponse(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _productService.DeleteAsync(id);
        if (result.IsSuccess)
        {
            await _vectorSearchService.DeleteAsync(id);
        }

        return CreateHttpResponse<ProductDTO>(result, 204);
    }
    #endregion

    public record AdjustmentRequest(int Quantity, InventoryOperation Type, ProductVariant Dims);
    [HttpPost("{id}/inventory"), Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> AdjustInventory(Guid id, [FromBody] AdjustmentRequest model)
    {
        var result = await _productService.GetByIdAsync(id);
        if (result.IsFailed) return CreateHttpResponse(result);

        result = await _inventoryService.UpdateAsync(result.Value, model.Dims, (model.Quantity, model.Type));
        return CreateHttpResponse(result, 201);
    }

    [HttpDelete("{id}/inventory"), Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> DeleteVariant(Guid id, [FromBody] ProductVariant dims)
    {
        var result = await _productService.GetByIdAsync(id);
        if (result.IsFailed) return CreateHttpResponse(result);

        result = await _inventoryService.DeleteAsync(result.Value, dims);
        return CreateHttpResponse(result, 201);
    }
}
