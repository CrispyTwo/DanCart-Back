using DanCart.DataAccess.Models;
using DanCart.Models.Auth;
using DanCart.WebApi.Areas.Customers.DTOs.Metrics;
using FluentResults;

namespace DanCart.WebApi.Areas.Customers.Services.IServices;

public interface ICustomerService
{
    public Task<Result<IEnumerable<ApplicationUser>>> GetAsync(int page, int pageSize, bool? isActive, string? sort);
    public Task<Result<IEnumerable<CustomerWithSalesInfoResponse>>> GetCustomersWithSalesAsync(int page, int pageSize, bool? isActive, string? sort);
    public Task<Result<ApplicationUser>> GetByIdAsync(Guid id);
    public Task<Result<long>> GetTotalAsync();
    public Task<Result<long>> GetActiveAsync();
    public Task<Result<long>> GetNewAsync();
}
