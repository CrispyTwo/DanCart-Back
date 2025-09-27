using DanCart.Models;
using DanCart.Models.DTOs.SalesOrder;
using FluentResults;

namespace DanCart.WebApi.Services.IServices;

public interface IOrderService
{
    public Task<Result<IEnumerable<SalesOrder>>> GetAsync(string userId, int page, int pageSize);
    public Task<Result<SalesOrder?>> GetAsync(string userId, Guid id);
    public Task<Result<SalesOrder>> CreateAsync(string userId, SalesOrderCreateDTO model);
}