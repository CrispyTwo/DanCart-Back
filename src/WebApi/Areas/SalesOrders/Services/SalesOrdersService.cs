using AutoMapper;
using AutoMapper.QueryableExtensions;
using DanCart.DataAccess.Extensions;
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
using Stripe;
using System.Linq.Expressions;

namespace DanCart.WebApi.Areas.SalesOrders.Services;

public class SalesOrdersService(IUnitOfWork _unitOfWork, IMapper _mapper) : ServiceBase, ISalesOrdersService
{
    #region CRUD APIs
    public async Task<Result<IEnumerable<SalesOrderWithLinesDTO>>> GetAsync(string? userId, Page page, string? sort, bool isAdmin)
    {
        const int MinPageSize = 5, MaxPageSize = 25;
        page.ApplySizeRule(MinPageSize, MaxPageSize);

        var orders = _unitOfWork.SalesOrder.GetQuery()
            .Include(x => x.SalesLines)
            .ProjectTo<SalesOrderWithLinesDTO>(_mapper.ConfigurationProvider);

        return Result.Ok(await orders.GetPageAsync(page, BuildSortingMap(sort)));
    }

    public async Task<Result<SalesOrderWithLinesDTO>> GetAsync(string userId, Guid id, bool isAdmin)
    {
        var order = await _unitOfWork.SalesOrder.GetQuery()
            .Where(isAdmin ? x => x.Id == id : x => x.Id == id && x.UserId == userId)
            .Include(x => x.SalesLines)
            .ProjectTo<SalesOrderWithLinesDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            return Result.Fail<SalesOrderWithLinesDTO>(
                new Error($"Order with specified ID doesn't exist")
                    .WithMetadata(ErrorMetadata.Code, ErrorCode.NotFound));
        }

        return Result.Ok(order);
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
    #endregion

    public async Task<Result<SalesOrderWithLinesDTO>> CreateAsync(string userId, SalesOrderCreateDTO model, IEnumerable<ShoppingCart> cart)
    {
        var salesOrder = _mapper.Map<SalesOrder>(model);
        var lines = _mapper.Map<List<SalesLine>>(cart);
        salesOrder.UserId = userId;
        foreach (var line in lines)
        {
            var product = await _unitOfWork.Product.GetAsync(x => x.Id == line.ProductId, tracked: true);
            if (product == null)
            {
                return DefaultNotFound<SalesOrderWithLinesDTO>(line.ProductId, nameof(Models.Products.Product));
            }

            var inventory = await _unitOfWork.Inventory.GetAsync(x => x.ProductId == product.Id && x.Color == line.Color && x.Size == line.Size, tracked: true);
            if (inventory == null || line.Quantity > inventory.Quantity)
            {
                return Result.Fail<SalesOrderWithLinesDTO>(
                    new Error($"Insufficient stock for product {product.Name}. Available: {product.Inventory.Sum(x => x.Quantity)}, Requested: {line.Quantity}")
                        .WithMetadata(ErrorMetadata.Code, ErrorCode.Conflict));
            }

            line.SalesOrder = salesOrder;
            line.UnitPrice = product.Price;
            inventory.Quantity -= line.Quantity;

            await _unitOfWork.SalesLine.AddAsync(line);
        }

        if (!await _unitOfWork.TrySaveAsync())
            return DefaultServerError<SalesOrderWithLinesDTO>();

        var result = _mapper.Map<SalesOrderWithLinesDTO>(salesOrder);
        return Result.Ok(result);
    }

    public async Task<Result> UpdateStatus(Guid id)
    {
        var salesOrder = await _unitOfWork.SalesOrder.GetQuery(tracking: true).FirstOrDefaultAsync(x => x.Id == id);
        if (salesOrder == null)
            return Result.Fail(new Error($"No order with id: {id} exists")
                .WithMetadata(ErrorMetadata.Code, ErrorCode.NotFound));

        salesOrder.OrderStatus = salesOrder.OrderStatus + 1;

        await _unitOfWork.SaveAsync();
        return Result.Ok();
    }

    public async Task<Result> SucceedPayment(string paymentIntent)
    {
        var salesOrder = await _unitOfWork.SalesOrder.GetQuery(tracking: true).FirstOrDefaultAsync(x => x.PaymentIntendId == paymentIntent);
        if (salesOrder == null)
            return Result.Fail(new Error($"No order with payment intent: {paymentIntent} exists")
                .WithMetadata(ErrorMetadata.Code, ErrorCode.NotFound));

        salesOrder.OrderStatus = SalesOrderStatus.Processing;
        salesOrder.PaymentStatus = SalesOrderPaymentStatus.Completed;
        salesOrder.PaymentDate = DateTime.UtcNow;

        await _unitOfWork.SaveAsync();
        return Result.Ok();
    }
}
