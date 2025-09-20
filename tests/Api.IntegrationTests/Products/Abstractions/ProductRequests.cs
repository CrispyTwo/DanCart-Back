using DanCart.Models.DTOs.Authentication;
using DanCart.Models.DTOs.Product;
using DanCart.Utility;

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
        WeightUnit = UnitsOfMeasure.Kilogram,
    };

    internal static ProductUpdateDTO GetCreateProduct2() => new()
    {
        Name = "Box",
        Description = "Misterious Box",
        Price = 12.5M,
        Stock = 200,
        LowStockThreshold = 15,
        Weight = 1M,
        WeightUnit = UnitsOfMeasure.Kilogram,
    };
}
