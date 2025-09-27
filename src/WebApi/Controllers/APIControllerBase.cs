using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace DanCart.WebApi.Controllers;

public class APIControllerBase : ControllerBase
{
    public const string Code = "Code";
    internal IActionResult? ProcessGenericError<T>(Result<T> result)
    {
        if (result.IsSuccess) return null;

        var error = result.Errors[0];
        if (!error.Metadata.TryGetValue(Code, out var code)) return null;

        switch (code)
        {
            case ErrorCode.NotFound:
                return NotFound(error.Message);
            default:
                return null;
        }
    }

    public enum ErrorCode
    {
        NotFound
    }
}