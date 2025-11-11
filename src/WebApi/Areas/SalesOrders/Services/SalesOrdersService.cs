using AutoMapper;
using DanCart.DataAccess.Models.Utility;
using DanCart.DataAccess.Repository;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models.Products;
using DanCart.Models.SalesOrders;
using DanCart.WebApi.Areas.SalesOrders.DTOs;
using DanCart.WebApi.Areas.SalesOrders.Services.IServices;
using DanCart.WebApi.Core;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DanCart.WebApi.Areas.SalesOrders.Services;

public class SalesOrdersService(IUnitOfWork _unitOfWork, IMapper _mapper) : ServiceBase, ISalesOrdersService
{
    public async Task<Result<IEnumerable<SalesOrderWithLinesDTO>>> GetAsync(string? userId, int page, int pageSize, bool isAdmin)
    {
        const int MinPageSize = 5, MaxPageSize = 25;
        pageSize = GetPageSize(pageSize, MinPageSize, MaxPageSize);

        var options = new GetRangeOptions<SalesOrder>();
        if (!isAdmin) options.Filter = o => o.UserId == userId;

        var orders = await _unitOfWork.SalesOrder.GetRangeAsync(new Page { Number = page, Size = pageSize }, options);
        var orderIds = orders.Select(o => o.Id).ToHashSet();

        var lines = await _unitOfWork.SalesLine.GetQuery().AsNoTracking()
            .Where(sl => orderIds.Contains(sl.SalesOrderId))
            .ToListAsync();

        var linesByOrder = lines.GroupBy(l => l.SalesOrderId).ToDictionary(g => g.Key, g => g.ToList());
        var result = _mapper.Map<IEnumerable<SalesOrderWithLinesDTO>>(orders);
        foreach (var orderDto in result)
        {
            orderDto.Lines = linesByOrder[orderDto.Id] ?? [];
            orderDto.Total = orderDto.Lines.Sum(x => x.Price * x.Quantity);
        }

        return Result.Ok(result);
    }

    public async Task<Result<SalesOrderWithLinesDTO>> GetAsync(string userId, Guid id, bool isAdmin)
    {
        Expression<Func<SalesOrder, bool>> filter = isAdmin ? x => x.Id == id : x => x.Id == id && x.UserId == userId;
        var order = await _unitOfWork.SalesOrder.GetAsync(filter);
        if (order == null)
        {
            return Result.Fail<SalesOrderWithLinesDTO>(
                new Error($"Order with specified ID doesn't exist")
                    .WithMetadata(ErrorMetadata.Code, ErrorCode.NotFound));
        }

        var lines = await _unitOfWork.SalesLine.GetQuery().AsNoTracking()
            .Where(sl => sl.SalesOrderId == order.Id)
            .ToListAsync();

        var result = _mapper.Map<SalesOrderWithLinesDTO>(order);
        result.Lines = lines;
        result.Total = lines.Sum(x => x.Price * x.Quantity);

        return Result.Ok(result);
    }

    public async Task<Result<SalesOrderWithLinesDTO>> CreateAsync(string userId, SalesOrderCreateDTO model)
    {
        var entity = _mapper.Map<SalesOrder>(model);
        entity.UserId = userId;
        await _unitOfWork.SalesOrder.AddAsync(entity);

        var lines = _mapper.Map<SalesLine[]>(model.Lines);
        foreach (var line in lines)
        {
            var product = await _unitOfWork.Product.GetAsync(x => x.Id == line.ProductId, tracked: true);
            if (product == null)
            {
                return DefaultNotFound<SalesOrderWithLinesDTO>(line.ProductId, nameof(Product));
            }

            if (line.Quantity > product.Stock)
            {
                return Result.Fail<SalesOrderWithLinesDTO>(
                    new Error($"Insufficient stock for product {product.Name}. Available: {product.Stock}, Requested: {line.Quantity}")
                        .WithMetadata(ErrorMetadata.Code, ErrorCode.Conflict));
            }

            line.SalesOrder = entity;
            line.Price = product.Price;
            product.Stock -= line.Quantity;

            await _unitOfWork.SalesLine.AddAsync(line);
        }

        if (!await _unitOfWork.TrySaveAsync())
            return DefaultServerError<SalesOrderWithLinesDTO>();

        var result = _mapper.Map<SalesOrderWithLinesDTO>(entity);
        return Result.Ok(result);
    }

    public async Task<Result> DeleteAsync(string userId, Guid id, bool isAdmin)
    {
        Expression<Func<SalesOrder, bool>> filter = isAdmin ? x => x.Id == id : x => x.Id == id && x.UserId == userId;
        var order = await _unitOfWork.SalesOrder.GetAsync(filter);
        if (order == null)
        {
            return DefaultNotFound(id, nameof(SalesOrder));
        }

        if (order.OrderStatus != SalesOrderStatus.Created)
        {
            return Result.Fail(
                new Error($"Order is already being processed, please contact the support for more details on how to cancel it")
                    .WithMetadata(ErrorMetadata.Code, ErrorCode.Conflict));
        }

        _unitOfWork.SalesOrder.Remove(order);

        if (!await _unitOfWork.TrySaveAsync())
            return DefaultServerError();

        return Result.Ok();
    }
}
