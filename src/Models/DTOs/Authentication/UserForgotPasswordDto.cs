using System.ComponentModel.DataAnnotations;

namespace DanCart.Models.DTOs.Authentication;

public class UserForgotPasswordDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
}
