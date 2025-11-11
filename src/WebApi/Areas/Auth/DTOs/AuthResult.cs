namespace DanCart.WebApi.Areas.Auth.DTOs;

public class AuthResult
{
    public required string User { get; set; }
    public required string Token { get; set; }
}
