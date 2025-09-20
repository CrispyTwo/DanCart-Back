namespace DanCart.WebApi.Extensions;

using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub")
            ?? user.FindFirstValue("oid");

        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException("Unauthrized access.");

        return userId;
    }
}
