using DanCart.Models.Auth;

namespace DanCart.WebApi.Areas.Auth.Services.IServices;

public interface ITokenProviderService
{
    public Task<string> GenerateJwtToken(ApplicationUser user);
    public Task<string> GenerateRefreshToken(string userId);
    public Task<(ApplicationUser user, string token)?> RefreshJwtToken(string refreshToken);
}
