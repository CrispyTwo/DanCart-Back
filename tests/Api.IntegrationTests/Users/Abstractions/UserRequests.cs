using DanCart.WebApi.Areas.Auth.DTOs;
using static DanCart.WebApi.Areas.Auth.Controllers.AuthController;

namespace Api.FunctionalTests.Users.Abstractions;

internal class UserRequests
{
    internal const string BaseUrl = "/api/v1/Auth";
    internal static LoginRequest GetLoginAdmin => new("Admin@gmail.com", "Admin123*");

    internal static UserRegisterRequest GetRegistrationUser1 => new()
    {
        FirstName = "Ibrahim",
        LastName = "Sanches",
        Email = "ibrahim.sanches@gmail.com",
        Phone = "123456789",
        Password = "Password123!",
        ConfirmPassword = "Password123!"
    };

    internal static LoginRequest GetLoginUser1 => new(GetRegistrationUser1.Email, GetRegistrationUser1.Password);

    internal static UserRegisterRequest GetRegistrationUser2 => new()
    {
        FirstName = "Artem",
        LastName = "Kozlov",
        Email = "artem.kozlov@gmail.com",
        Phone = "12357431",
        Password = "123456789/Qw",
        ConfirmPassword = "123456789/Qw"
    };

    internal static LoginRequest GetLoginUser2 => new(GetRegistrationUser2.Email, GetRegistrationUser2.Password);
}