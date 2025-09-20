
namespace Api.FunctionalTests.Abstractions;

public class BaseFunctionalTest(FunctionalTestWebAppFactory factory) : IClassFixture<FunctionalTestWebAppFactory>, IAsyncLifetime
{
    protected HttpClient HttpClient { get; init; } = factory.HttpClient;
    protected Func<Task> ResetDatabase { get; init; } = factory.ResetDatabaseAsync;

    public Task DisposeAsync() => ResetDatabase();
    public Task InitializeAsync() => Task.CompletedTask;
}
