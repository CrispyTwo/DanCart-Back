using Api.FunctionalTests.Users.Abstractions;
using DanCart.Models.DTOs.SalesOrder;

namespace Api.FunctionalTests.SalesOrders.Abstractions;

internal class SalesOrderRequests
{
    internal const string BaseUrl = "/api/v1/Orders";
    internal static SalesOrderCreateDTO GetCreateSalesOrder1 => new()
    {
        Lines = [],
        Email = UserRequests.GetRegistrationUser1.Email,
        Name = "Someone",
        Phone = "123456789",
        City = "City",
        Country = "Country",
        Region = "Region",
        Street = "Street 123"
    };
}
