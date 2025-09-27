using AutoMapper;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models;
using DanCart.Models.DTOs.SalesOrder;
using DanCart.WebApi.Services.IServices;
using FluentResults;

namespace DanCart.WebApi.Services;

public class OrderService(IUnitOfWork _unitOfWork, IMapper _mapper) : ServiceBase, IOrderService
{
    public async Task<Result<IEnumerable<SalesOrder>>> GetAsync(string userId, int page, int pageSize)
    {
        const int MinPageSize = 5, MaxPageSize = 25;
        pageSize = GetPageSize(pageSize, MinPageSize, MaxPageSize);
        return Result.Ok(await _unitOfWork.SalesOrder.GetRangeAsync(page, pageSize, o => o.UserId == userId));
    }

    public async Task<Result<SalesOrder?>> GetAsync(string userId, Guid id)
    {
        return Result.Ok(await _unitOfWork.SalesOrder.GetAsync(x => x.Id == id && x.UserId == userId));
    }

    public async Task<Result<SalesOrder>> CreateAsync(string userId, SalesOrderCreateDTO model)
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
                return Result.Fail<SalesOrder>(
                    new Error($"Product with ID {line.ProductId} does not exist.")
                        .WithMetadata("Code", "NotFound"));
            }

            if (line.Count > product.Stock)
            {
                return Result.Fail<SalesOrder>(
                    new Error($"Insufficient stock for product {product.Name}. Available: {product.Stock}, Requested: {line.Count}")
                        .WithMetadata("Code", "Conflict"));
            }

            line.SalesOrder = entity;
            line.Price = product.Price;
            product.Stock -= line.Count;

            await _unitOfWork.SalesLine.AddAsync(line);
        }

        try
        {
            await _unitOfWork.SaveAsync();
        }
        catch
        {
            return Result.Fail<SalesOrder>(
                new Error("Failed to save order.")
                    .WithMetadata("Code", "ServerError"));
        }

        return Result.Ok(entity);
    }
}
