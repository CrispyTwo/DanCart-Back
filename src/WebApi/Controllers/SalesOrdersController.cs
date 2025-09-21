using AutoMapper;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models;
using DanCart.Models.DTOs.SalesOrder;
using DanCart.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanCart.WebApi.Controllers;

[ApiController, Route("api/v1/[controller]"), Authorize]
public class SalesOrdersController(IUnitOfWork _unitOfWork, IMapper _mapper) : RESTControllerBase
{
    private const int MaxPageSize = 25;
    private const int MinPageSize = 5;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = MinPageSize)
    {
        var userId = User.GetUserId();

        pageSize = GetPageSize(pageSize, MinPageSize, MaxPageSize);
        var orders = await _unitOfWork.SalesOrder.GetRangeAsync(page, pageSize, o => o.UserId == userId);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var userId = User.GetUserId();

        var order = await _unitOfWork.SalesOrder.GetAsync(x => x.Id == id && x.UserId == userId);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SalesOrderCreateDTO model)
    {
        var entity = _mapper.Map<SalesOrder>(model);
        entity.UserId = User.GetUserId();
        await _unitOfWork.SalesOrder.AddAsync(entity);

        var lines = _mapper.Map<SalesLine[]>(model.Lines);
        foreach (var line in lines)
        {
            var product = await _unitOfWork.Product.GetAsync(x => x.Id == line.ProductId, tracked: true);
            if (product == null)
            {
                return BadRequest($"Product with ID {line.ProductId} does not exist.");
            }

            line.SalesOrderId = entity.Id;
            line.Price = product.Price;

            if (line.Count > product.Stock)
            {
                return BadRequest($"Insufficient stock for product {product.Name}. Available stock: {product.Stock}");
            }

            product.Stock -= line.Count;
            await _unitOfWork.SalesLine.AddAsync(line);
        }

        await _unitOfWork.SaveAsync();
        return CreatedAtAction(nameof(Create), entity);
    }
}
