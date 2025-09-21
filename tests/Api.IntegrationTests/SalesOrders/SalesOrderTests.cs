using Api.FunctionalTests.Abstractions;
using DanCart.Models.DTOs.SalesLine;
using System.Net.Http.Json;
using DanCart.Models;
using FluentAssertions;
using Api.FunctionalTests.Products.Abstractions;
using Api.FunctionalTests.SalesOrders.Abstractions;

namespace Api.FunctionalTests.SalesOrders;

public class SalesOrderTests : BaseFunctionalTest, IAsyncLifetime
{
    private readonly ProductFunctions _productFunctions;
    public SalesOrderTests(FunctionalTestWebAppFactory factory) : base(factory)
    {
        _productFunctions = new ProductFunctions(HttpClient);
    }

    [Fact]
    public async Task CreateSalesOrderV1_UnauthorizedUser_StockDeducted()
    {
        Guid id = await _productFunctions.CreateProduct1(5);

        var request = SalesOrderRequests.GetCreateSalesOrder1;
        request.Lines = [new() { ProductId = id, Count = 2 }];

        // Act
        var response = await HttpClient.PostAsJsonAsync(SalesOrderRequests.BaseUrl, request);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateSalesOrderV1_UnexistentProductId_BadRequest()
    {
        var request = SalesOrderRequests.GetCreateSalesOrder1;
        request.Lines = [new() { ProductId = Guid.NewGuid(), Count = 2 }];

        using (HttpClient.UseBearer())
        {
            // Act
            var response = await HttpClient.PostAsJsonAsync(SalesOrderRequests.BaseUrl, request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }

    [Fact]
    public async Task CreateSalesOrderV1_LinesWithSameProductId_DuplicatedPKAndServerError()
    {
        Guid id = await _productFunctions.CreateProduct1(5);
        var salesLines = new[]
        {
            new SalesLineCreateDTO { ProductId = id, Count = 2 },
            new SalesLineCreateDTO { ProductId = id, Count = 1 }
        };

        using (HttpClient.UseBearer())
        {
            var request = SalesOrderRequests.GetCreateSalesOrder1;
            request.Lines = salesLines;

            // Act
            var response = await HttpClient.PostAsJsonAsync(SalesOrderRequests.BaseUrl, request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
        }
    }

    [Fact]
    public async Task CreateSalesOrderV1_EnoughStock_StockDeducted()
    {
        Guid id1 = await _productFunctions.CreateProduct1(5), id2 = await _productFunctions.CreateProduct2(5);
        var salesLines = new[] 
        {
            new SalesLineCreateDTO { ProductId = id1, Count = 2 },
            new SalesLineCreateDTO { ProductId = id2, Count = 1 }
        };

        using (HttpClient.UseBearer())
        {
            var request = SalesOrderRequests.GetCreateSalesOrder1;
            request.Lines = salesLines;

            var response = await HttpClient.PostAsJsonAsync(SalesOrderRequests.BaseUrl, request);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        // Act 
        var getItem1 = await HttpClient.GetAsync($"{ProductRequests.BaseUrl}/{id1}");
        var getItem2 = await HttpClient.GetAsync($"{ProductRequests.BaseUrl}/{id2}");

        // Assert
        (await getItem1.Content.ReadFromJsonAsync<Product>())!.Stock.Should().Be(3);
        (await getItem2.Content.ReadFromJsonAsync<Product>())!.Stock.Should().Be(4);
    }

    [Fact]
    public async Task CreateSalesOrderV1_OneItemNotEnoughStock_RequestFailed()
    {
        Guid id1 = await _productFunctions.CreateProduct1(5), id2 = await _productFunctions.CreateProduct2(5);
        var salesLines = new[]
        {
            new SalesLineCreateDTO { ProductId = id1, Count = 20 },
            new SalesLineCreateDTO { ProductId = id2, Count = 1 }
        };

        using (HttpClient.UseBearer())
        {
            var request = SalesOrderRequests.GetCreateSalesOrder1;
            request.Lines = salesLines;

            var response = await HttpClient.PostAsJsonAsync(SalesOrderRequests.BaseUrl, request);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        // Act 
        var getItem1 = await HttpClient.GetAsync($"{ProductRequests.BaseUrl}/{id1}");
        var getItem2 = await HttpClient.GetAsync($"{ProductRequests.BaseUrl}/{id2}");

        // Assert
        (await getItem1.Content.ReadFromJsonAsync<Product>())!.Stock.Should().Be(5);
        (await getItem2.Content.ReadFromJsonAsync<Product>())!.Stock.Should().Be(5);
    }
}
