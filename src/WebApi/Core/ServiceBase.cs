using FluentResults;

namespace DanCart.WebApi.Core;

public class ServiceBase
{
    protected IEnumerable<(string Name, bool Desc)> BuildSortingMap(string? sorts)
    {
        if (string.IsNullOrWhiteSpace(sorts)) yield break;

        foreach (var sorting in sorts.Split(','))
        {
            var sortParts = sorting.Split(':');

            bool desc = false;
            if (sortParts.Length > 1)
            {
                desc = string.Equals(sortParts[1], "desc", StringComparison.CurrentCultureIgnoreCase) ||
                     string.Equals(sortParts[1], "descending", StringComparison.CurrentCultureIgnoreCase);
            }

            yield return (sortParts[0], desc);
        }
    }
    protected Result<T> DefaultNotFound<T>(Guid id, string? entity = null)
    {
        entity ??= typeof(T).Name;
        return Result.Fail<T>(
            new Error($"{entity} with specific id: {id} not found")
                .WithMetadata(ErrorMetadata.Code, ErrorCode.NotFound));
    }
    protected Result DefaultNotFound(Guid id, string entity)
    {
        return Result.Fail(
            new Error($"{entity} with specific id: {id} not found")
                .WithMetadata(ErrorMetadata.Code, ErrorCode.NotFound));
    }
    protected Result<T> DefaultServerError<T>()
    {
        return Result.Fail<T>(
            new Error("Failed to accomplish action.")
                .WithMetadata(ErrorMetadata.Code, ErrorCode.ServerError));
    }
    protected Result DefaultServerError()
    {
        return Result.Fail(
            new Error("Failed to accomplish action.")
                .WithMetadata(ErrorMetadata.Code, ErrorCode.ServerError));
    }
}
