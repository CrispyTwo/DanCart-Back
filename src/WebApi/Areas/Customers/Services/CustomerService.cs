using AutoMapper;
using AutoMapper.QueryableExtensions;
using DanCart.DataAccess.Extensions;
using DanCart.DataAccess.Models.Utility;
using DanCart.DataAccess.Repository;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models.Auth;
using DanCart.WebApi.Areas.Customers.DTOs.Metrics;
using DanCart.WebApi.Areas.Customers.Services.IServices;
using DanCart.WebApi.Core;
using FluentResults;
using System.Linq.Expressions;

namespace DanCart.WebApi.Areas.Customers.Services;

public class CustomerService(IUnitOfWork _unitOfWork, IMapper _mapper) : ServiceBase, ICustomerService
{
    public async Task<Result<IEnumerable<CustomerWithSalesInfoResponse>>> GetCustomersWithSalesAsync(
        Page page, bool? isActive, string? sort, string? search)
    {
        const int MinPageSize = 10, MaxPageSize = 50;
        page.ApplySizeRule(MinPageSize, MaxPageSize);

        var query = _unitOfWork.ApplicationUser.GetQuery();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.ApplyFullTextSearch(search).OrderByDescending(x => x.Rank).Select(x => x.Entity);
            

        var projected = query.ProjectTo<CustomerWithSalesInfoResponse>(_mapper.ConfigurationProvider);

        if (isActive.HasValue) projected = projected.Where(x => x.IsActive == isActive.Value);
        return Result.Ok(await projected.GetPageAsync(page, BuildSortingMap(sort)));
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
