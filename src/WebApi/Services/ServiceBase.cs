using DanCart.Models;
using DanCart.WebApi.Controllers;
using FluentResults;

namespace DanCart.WebApi.Services;

public class ServiceBase
{
    protected int GetPageSize(int pageSize, int minPageSize, int maxPageSize) => Math.Clamp(pageSize, minPageSize, maxPageSize);
    protected Result<T> DefaultNotFound<T>(Guid id)
    {
        return Result.Fail<T>(
            new Error($"{nameof(T)} with specific id: {id} not found")
                .WithMetadata(APIControllerBase.Code, APIControllerBase.ErrorCode.NotFound));
    }
}
