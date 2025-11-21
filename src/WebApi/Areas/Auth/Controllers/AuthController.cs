using AutoMapper;
using DanCart.Models.Auth;
using DanCart.WebApi.Areas.Auth.DTOs;
using DanCart.WebApi.Areas.Auth.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DanCart.WebApi.Areas.Auth.Controllers;

[ApiController, Route("api/v1/[controller]"), Authorize(Roles = UserRole.Admin)]
public class AuthController(
    UserManager<ApplicationUser> _userManager,
    SignInManager<ApplicationUser> _signInManager,
    ITokenProviderService _tokenProviderService, IMapper _mapper) : ControllerBase
{
    public sealed record AuthResult(string Email, string Token, string RefreshToken);

    [HttpPost("register"), AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest model)
    {
        if (model.Password != model.ConfirmPassword) return BadRequest("Passwords do not match.");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var entity = _mapper.Map<ApplicationUser>(model);

        var result = await _userManager.CreateAsync(entity, model.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        result = await _userManager.AddToRoleAsync(entity, UserRole.Customer);
        if (!result.Succeeded) return BadRequest(result.Errors);

        var token = await _tokenProviderService.GenerateJwtToken(entity);
        var refreshToken = await _tokenProviderService.GenerateRefreshToken(entity.Id);

        return Ok(new AuthResult(model.Email, token, refreshToken));
    }

    public sealed record LoginRequest(string Email, string Password);

    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return Unauthorized("Invalid credentials");

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded) return Unauthorized("Invalid credentials");

        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var token = await _tokenProviderService.GenerateJwtToken(user);
        var refreshToken = await _tokenProviderService.GenerateRefreshToken(user.Id);

        return Ok(new AuthResult(model.Email, token, refreshToken));
    }

    [HttpPost("refresh-token"), AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        var result = await _tokenProviderService.RefreshJwtToken(refreshToken);
        if (result == null) return Unauthorized("Invalid or expired refresh token");

        return Ok(new AuthResult(result.Value.user.Email!, result.Value.token, refreshToken));
    }
}
