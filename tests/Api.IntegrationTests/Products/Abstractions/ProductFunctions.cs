using Api.FunctionalTests.Abstractions;
using DanCart.Models.Products;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Products.Abstractions;

internal class ProductFunctions(HttpClient _client)
{
    internal async Task<Guid> CreateProduct1(int? stock = null, decimal? price = null)
    {
        var entity = ProductRequests.GetCreateProduct1();
        if (stock.HasValue) entity.Stock = stock.Value;
        if (price.HasValue) entity.Price = price.Value;

        return await CreateProduct(() => _client.PostAsJsonAsync(ProductRequests.BaseUrl, entity));
    }

    internal async Task<Guid> CreateProduct2(int? stock = null, decimal? price = null)
    {
        var entity = ProductRequests.GetCreateProduct2();
        if (stock.HasValue) entity.Stock = stock.Value;
        if (price.HasValue) entity.Price = price.Value;

        return await CreateProduct(() => _client.PostAsJsonAsync(ProductRequests.BaseUrl, entity));
    }

    private async Task<Guid> CreateProduct(Func<Task<HttpResponseMessage>> func)
    {
        using var _ = _client.UseBearer();
        var response = await func();
        var responseContent = await response.Content.ReadFromJsonAsync<Product>();
        return responseContent!.Id;
    }
}
