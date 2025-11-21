using DanCart.DataAccess.Models.Utility;
using DanCart.Models.SalesOrders;
using DanCart.WebApi.Areas.SalesOrders.DTOs;
using FluentResults;

namespace DanCart.WebApi.Areas.SalesOrders.Services.IServices;

public interface ISalesOrdersService
{
    public Task<Result<IEnumerable<SalesOrderWithLinesDTO>>> GetAsync(string userId, Page page, string? sort, bool isAdmin);
    public Task<Result<SalesOrderWithLinesDTO>> GetAsync(string userId, Guid id, bool isAdmin);
    public Task<Result<SalesOrderWithLinesDTO>> CreateAsync(string userId, SalesOrderCreateDTO model);
    public Task<Result> DeleteAsync(string userId, Guid id, bool isAdmin);
}