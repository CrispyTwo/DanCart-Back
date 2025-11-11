using DanCart.DataAccess.Models;
using DanCart.DataAccess.Models.Utility;
using DanCart.DataAccess.Repository;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models.Auth;
using DanCart.WebApi.Areas.Customers.DTOs.Metrics;
using DanCart.WebApi.Areas.Customers.Services.IServices;
using DanCart.WebApi.Core;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Frozen;
using System.Linq.Expressions;

namespace DanCart.WebApi.Areas.Customers.Services;

public class CustomerService(IUnitOfWork _unitOfWork) : ServiceBase, ICustomerService
{
    public async Task<Result<IEnumerable<ApplicationUser>>> GetAsync(
        int page, int pageSize, bool? isActive, string? sort)
    {
        const int MinPageSize = 10, MaxPageSize = 50;
        pageSize = GetPageSize(pageSize, MinPageSize, MaxPageSize);

        Expression<Func<ApplicationUser, bool>>? filter = null;
        if (isActive.HasValue) filter = x => x.IsActive == isActive.Value;

        var sortings = BuildSortingMap(sort);
        var options = new GetRangeOptions<ApplicationUser>() { Filter = filter, Sortings = sortings };

        return Result.Ok(await _unitOfWork.ApplicationUser.GetRangeAsync(new Page { Number = page, Size = pageSize }, options));
    }

    public async Task<Result<IEnumerable<CustomerWithSalesInfoResponse>>> GetCustomersWithSalesAsync(
        int page, int pageSize, bool? isActive, string? sort)
    {
        var projected = _unitOfWork.ApplicationUser.GetQuery().Select(u => new CustomerWithSalesInfoResponse
        {
            FirstName = u.Name,
            LastName = u.LastName,
            Email = u.Email!,
            CreatedAt = u.CreatedAt,
            IsActive = u.IsActive,
            OrdersCount = _unitOfWork.SalesOrder.GetQuery().LongCount(x => x.UserId == u.Id),
            TotalSpent = _unitOfWork.SalesLine.GetQuery().Where(l => l.SalesOrder.UserId == u.Id)
                            .Select(l => (decimal?)(l.Price * l.Quantity))
                            .Sum() ?? 0m
        });

        //if (!string.IsNullOrWhiteSpace(sort))
        //{
        //    projected = ApplySorting(projected, sort);
        //}

        var result = await projected
            .Skip((Math.Max(1, page) - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync() as IEnumerable<CustomerWithSalesInfoResponse>;

        return Result.Ok(result);
    }


    public async Task<Result<ApplicationUser>> GetByIdAsync(Guid id)
    {
        var ApplicationUser = await _unitOfWork.ApplicationUser.GetAsync(x => x.Id == id.ToString());
        if (ApplicationUser == null) return DefaultNotFound<ApplicationUser>(id);

        return Result.Ok(ApplicationUser);
    }

    public async Task<Result<long>> GetActiveAsync()
    {
        return Result.Ok(await _unitOfWork.ApplicationUser.GetTotalAsync(x => x.IsActive == true));
    }
    public async Task<Result<long>> GetNewAsync()
    {
        return Result.Ok(await _unitOfWork.ApplicationUser.GetTotalAsync(x => x.CreatedAt >= DateTime.UtcNow.AddDays(-30)));
    }

    public async Task<Result<long>> GetTotalAsync()
    {
        return Result.Ok(await _unitOfWork.ApplicationUser.GetTotalAsync());
    }
}
