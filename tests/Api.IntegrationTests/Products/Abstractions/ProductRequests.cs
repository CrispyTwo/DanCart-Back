using DanCart.Models.Products;
using DanCart.Products.Models.DTOs;
using DanCart.WebApi.Areas.Products.DTOs;

namespace Api.FunctionalTests.Products.Abstractions;

internal class ProductRequests
{
    internal const string BaseUrl = "/api/v1/Products";
    internal static ProductCreateDTO GetCreateProduct1() => new()
    {
        Name = "Toy",
        Description = "Kid's toy",
        Price = 19.99M,
        Stock = 100,
        LowStockThreshold = 10,
        Weight = 0.5M,
        WeightUnit = UnitOfMeasure.Kg,
    };

    internal static ProductUpdateDTO GetCreateProduct2() => new()
    {
        Name = "Box",
        Description = "Misterious Box",
        Price = 12.5M,
        Stock = 200,
        LowStockThreshold = 15,
        Weight = 1M,
        WeightUnit = UnitOfMeasure.Kg,
    };
}
