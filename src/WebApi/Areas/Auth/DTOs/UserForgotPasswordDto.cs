using System.ComponentModel.DataAnnotations;

namespace DanCart.WebApi.Areas.Auth.DTOs;

public class UserForgotPasswordDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
}
