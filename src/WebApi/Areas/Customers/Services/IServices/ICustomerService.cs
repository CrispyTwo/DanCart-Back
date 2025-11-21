using DanCart.DataAccess.Models.Utility;
using DanCart.Models.Auth;
using DanCart.WebApi.Areas.Customers.DTOs.Metrics;
using FluentResults;

namespace DanCart.WebApi.Areas.Customers.Services.IServices;

public interface ICustomerService
{
    public Task<Result<IEnumerable<ApplicationUser>>> GetAsync(Page page, bool? isActive, string? sort);
    public Task<Result<IEnumerable<CustomerWithSalesInfoResponse>>> GetCustomersWithSalesAsync(Page page, bool? isActive, string? sort);
    public Task<Result<ApplicationUser>> GetByIdAsync(Guid id);
    public Task<Result<long>> GetTotalAsync();
    public Task<Result<long>> GetActiveAsync();
    public Task<Result<long>> GetNewAsync();
}
