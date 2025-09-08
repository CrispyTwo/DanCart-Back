using System.ComponentModel.DataAnnotations;

namespace DanCart.Models.DTOs.Authentication;
public class UserLoginDTO
{
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
