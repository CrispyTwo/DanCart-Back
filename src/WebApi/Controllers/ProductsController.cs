using AutoMapper;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models;
using DanCart.Models.DTOs.Product;
using DanCart.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanCart.WebApi.Controllers;

[ApiController, Route("api/v1/[controller]"), Authorize(Roles = UserRole.Admin)]
public class ProductsController(IUnitOfWork _unitOfWork, IMapper _mapper) : RESTControllerBase
{
    private const int MaxPageSize = 100;
    private const int MinPageSize = 20;

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = MinPageSize)
    {
        pageSize = GetPageSize(pageSize, MinPageSize, MaxPageSize);
        var products = await _unitOfWork.Product.GetRangeAsync(page, pageSize);
        return Ok(products);
    }

    [HttpGet("{id}"), AllowAnonymous]
    public async Task<IActionResult> Get(Guid id)
    {
        var product = await _unitOfWork.Product.GetAsync(x => x.Id == id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var entity = _mapper.Map<Product>(model);

        await _unitOfWork.Product.AddAsync(entity);
        await _unitOfWork.SaveAsync();

        return CreatedAtAction(nameof(Create), entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = await _unitOfWork.Product.GetAsync(x => x.Id == id);
        if (entity == null) return NotFound();
        _mapper.Map(model, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Product.Update(entity);
        await _unitOfWork.SaveAsync();
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _unitOfWork.Product.GetAsync(x => x.Id == id);
        if (product == null) return NotFound();

        _unitOfWork.Product.Remove(product);
        await _unitOfWork.SaveAsync();
        return NoContent();
    }

    //[HttpPost("import")]
    //public async Task<IActionResult> ImportProducts([FromForm] ImportProductsDto model)
    //{
    //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //    if (string.IsNullOrEmpty(userId))
    //        return Unauthorized();

    //    var result = await _productService.ImportProductsAsync(model, userId);
    //    return Ok(result);
    //}

    //[HttpPost("{id}/upload-image")]
    //public async Task<IActionResult> UploadProductImage(int id, IFormFile file)
    //{
    //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //    if (string.IsNullOrEmpty(userId))
    //        return Unauthorized();

    //    var imageUrl = await _productService.UploadProductImageAsync(id, file, userId);
    //    if (string.IsNullOrEmpty(imageUrl))
    //        return BadRequest("Failed to upload image");

    //    return Ok(new { imageUrl });
    //}
}
