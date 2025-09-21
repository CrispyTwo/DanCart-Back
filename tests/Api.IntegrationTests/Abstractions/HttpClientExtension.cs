using Api.FunctionalTests.Users.Abstractions;
using System.Net.Http.Headers;

namespace Api.FunctionalTests.Abstractions;

internal static class HttpClientExtension
{
    internal static TemporaryAuthorization UseBearer(this HttpClient client, string jwt = "")
        => new(client, jwt);

    internal sealed class TemporaryAuthorization : IDisposable
    {
        private readonly HttpClient _client;

        public TemporaryAuthorization(HttpClient client, string jwt)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            if (string.IsNullOrWhiteSpace(jwt)) jwt = new UserFunctions(_client).LoginAdmin().GetAwaiter().GetResult();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        public void Dispose()
        {
            _client.DefaultRequestHeaders.Remove("Authorization");
        }
    }
}
