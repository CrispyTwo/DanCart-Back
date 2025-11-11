using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace DanCart.WebApi.Core;

public class APIControllerBase : ControllerBase
{
    internal IActionResult CreateHttpResponse<T>(Result<T> result, int onSuccessCode = 200)
    {
        if (result.IsSuccess) 
            return StatusCode(onSuccessCode, result.Value);

        var error = result.Errors[0];
        if (!error.Metadata.TryGetValue(ErrorMetadata.Code, out var code))
            code = ErrorCode.ServerError;

        return code switch
        {
            ErrorCode.NotFound => NotFound(error.Message),
            ErrorCode.Conflict => Conflict(error.Message),
            ErrorCode.ServerError => StatusCode(500, error.Message),
            _ => BadRequest(error.Message),
        };
    }
}