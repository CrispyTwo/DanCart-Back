using DanCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Api.FunctionalTests.Users.Abstractions;

internal class UserFunctions(HttpClient _client)
{
    internal async Task<string> LoginAdmin() =>
        await ReadToken(() => _client.PostAsJsonAsync($"{UserRequests.BaseUrl}/login", UserRequests.GetLoginAdmin));

    internal async Task<string> CreateUser1() =>
        await ReadToken(() => _client.PostAsJsonAsync($"{UserRequests.BaseUrl}/register", UserRequests.GetRegistrationUser1));

    internal async Task<string> CreateUser2() => 
        await ReadToken(() => _client.PostAsJsonAsync($"{UserRequests.BaseUrl}/register", UserRequests.GetRegistrationUser2));

    private async Task<string> ReadToken(Func<Task<HttpResponseMessage>> func)
    {
        var token = await (await func())!.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(token)) throw new UnauthorizedAccessException();
        return token;
    }
}
