using DanCart.WebApi.Areas.Auth.DTOs;

namespace Api.FunctionalTests.Users.Abstractions;

internal class UserRequests
{
    internal const string BaseUrl = "/api/v1/Auth";
    internal static UserLoginDTO GetLoginAdmin => new()
    {
        Email = "Admin@gmail.com",
        Password = "Admin123*"
    };

    internal static UserRegisterDTO GetRegistrationUser1 => new()
    {
        Name = "Ibrahim",
        LastName = "Sanches",
        Email = "ibrahim.sanches@gmail.com",
        Phone = "123456789",
        Password = "Password123!",
        ConfirmPassword = "Password123!"
    };

    internal static UserLoginDTO GetLoginUser1 => new()
    {
        Email = GetRegistrationUser1.Email,
        Password = GetRegistrationUser1.Password
    };

    internal static UserRegisterDTO GetRegistrationUser2 => new()
    {
        Name = "Artem",
        LastName = "Kozlov",
        Email = "artem.kozlov@gmail.com",
        Phone = "12357431",
        Password = "123456789/Qw",
        ConfirmPassword = "123456789/Qw"
    };

    internal static UserLoginDTO GetLoginUser2 => new()
    {
        Email = GetRegistrationUser2.Email,
        Password = GetRegistrationUser2.Password
    };
}