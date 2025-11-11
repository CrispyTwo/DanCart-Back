using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Users;
using DanCart.Models.SalesOrders;
using DanCart.WebApi.Areas.SalesOrders.SalesLines.DTOs;
using System.Net.Http.Json;

namespace Api.FunctionalTests.SalesOrders.Abstractions;

internal class SalesOrderFunctions(HttpClient _client)
{
    internal async Task<Guid> CreateSalesOrder1(SalesLineCreateDTO[] lines, string jwt = "")
    {
        var entity = SalesOrderRequests.GetCreateSalesOrder1;
        entity.Lines = lines;

        using var _ = _client.UseBearer();
        var response = await _client.PostAsJsonAsync(SalesOrderRequests.BaseUrl, entity);
        return (await response.Content.ReadFromJsonAsync<SalesOrder>())!.Id;
    }
}
