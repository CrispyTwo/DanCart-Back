using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Users.Abstractions;
using FluentAssertions;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Users;

public class UserTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{

    [Fact]
    public async Task RegisterUserV1_EmailMissing_BadRequest()
    {
        var request = UserRequests.GetRegistrationUser1;
        request.Email = string.Empty;

        // Act
        var response = await HttpClient.PostAsJsonAsync($"{UserRequests.BaseUrl}/register", request);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RegisterUserV1_PhoneMissing_Ok()
    {
        var request = UserRequests.GetRegistrationUser1;
        request.Phone = string.Empty;

        // Act
        var response = await HttpClient.PostAsJsonAsync($"{UserRequests.BaseUrl}/register", request);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task RegisterUserV1_PasswordMissmatch_BadRequest()
    {
        var request = UserRequests.GetRegistrationUser1;
        request.ConfirmPassword = "OtherPassword";

        // Act
        var response = await HttpClient.PostAsJsonAsync($"{UserRequests.BaseUrl}/register", request);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UserV1_ValidPayload_UserCanLogIn()
    {
        var registerRequest = UserRequests.GetRegistrationUser1;
        var loginRequest = UserRequests.GetLoginUser1;

        var registerResponse = await HttpClient.PostAsJsonAsync($"{UserRequests.BaseUrl}/register", registerRequest);
        registerResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        // Act
        var loginResponse = await HttpClient.PostAsJsonAsync($"{UserRequests.BaseUrl}/login", loginRequest);

        // Assert
        loginResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task UserV1_WrongPassword_UserCannotLogIn()
    {
        var registerRequest = UserRequests.GetRegistrationUser1;
        var loginRequest = UserRequests.GetLoginUser1;
        loginRequest.Password = "OtherPassword";

        var registerResponse = await HttpClient.PostAsJsonAsync($"{UserRequests.BaseUrl}/register", registerRequest);
        registerResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        // Act
        var loginResponse = await HttpClient.PostAsJsonAsync($"{UserRequests.BaseUrl}/login", loginRequest);

        // Assert
        loginResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
}
