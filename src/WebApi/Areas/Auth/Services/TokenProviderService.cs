using DanCart.DataAccess.Repository.IRepository;
using DanCart.Models.Auth;
using DanCart.WebApi.Areas.Auth.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Core.Types;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DanCart.WebApi.Areas.Auth.Services;

public class TokenProviderService(
    UserManager<ApplicationUser> _userManager,
    IConfiguration _configuration,
    IUnitOfWork _unitOfWork) : ITokenProviderService
{
    public async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"]!);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshToken(string userId)
    {
        var token = new RefreshToken()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(1)
        };

        await _unitOfWork.RefreshToken.AddAsync(token);
        await _unitOfWork.SaveAsync();

        return token.Token;
    }

    public async Task<(ApplicationUser, string)?> RefreshJwtToken(string refreshToken)
    {
        var token = await _unitOfWork.RefreshToken.GetAsync(x => x.Token == refreshToken, includeProperties: "User");
        if (token == null || token.ExpiresAt < DateTime.UtcNow || token.User == null) return null;

        return (token.User, await GenerateJwtToken(token.User));
    }
}