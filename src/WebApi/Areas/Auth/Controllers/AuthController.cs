using AutoMapper;
using DanCart.WebApi.Areas.Auth.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DanCart.DataAccess.Models;
using DanCart.Models.Auth;

namespace DanCart.WebApi.Areas.Auth.Controllers;

[ApiController, Route("api/v1/[controller]"), Authorize(Roles = UserRole.Admin)]
public class AuthController(
    UserManager<ApplicationUser> _userManager,
    SignInManager<ApplicationUser> _signInManager,
    IConfiguration _configuration, IMapper _mapper) : ControllerBase
{
    [HttpPost("register"), AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO model)
    {
        if (model.Password != model.ConfirmPassword) return BadRequest("Passwords do not match.");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var entity = _mapper.Map<ApplicationUser>(model);

        var result = await _userManager.CreateAsync(entity, model.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        result = await _userManager.AddToRoleAsync(entity, UserRole.Customer);
        if (!result.Succeeded) return BadRequest(result.Errors);

        var token = await GenerateJwtToken(entity);
        return Ok(new AuthResult { User = model.Email, Token = token });
    }


    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return Unauthorized("Invalid credentials");

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded) return Unauthorized("Invalid credentials");

        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var token = await GenerateJwtToken(user);
        return Ok(new AuthResult { User = model.Email, Token = token });
    }

    //[HttpPost("forgot-password")]
    //public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
    //{
    //    var user = await _userManager.FindByEmailAsync(model.Email);
    //    if (user == null)
    //        return Ok(new { message = "If the email exists, a reset link has been sent." });

    //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    //    // Here you would send the email with the reset link
    //    // For now, we'll just return success

    //    return Ok(new { message = "If the email exists, a reset link has been sent." });
    //}

    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"]);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
