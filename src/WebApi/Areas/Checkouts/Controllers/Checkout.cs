using DanCart.WebApi.Areas.Checkouts.Services.IServices;
using DanCart.WebApi.Areas.SalesOrders.DTOs;
using DanCart.WebApi.Core;
using DanCart.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanCart.WebApi.Areas.Checkouts.Controllers;

[ApiController, Route("api/v1/[controller]"), Authorize]
public class CheckoutController(ICheckoutService _checkoutService) : APIControllerBase
{
    [HttpPost("initiate")]
    public async Task<IActionResult> InitiateCheckout([FromBody] SalesOrderCreateDTO userDetails)
    {
        var result = await _checkoutService.InitiateCheckout(User.GetUserId(), userDetails);
        return CreateHttpResponse(result);
    }

    public record SalesOrderPaymentDTO(string paymentIntent, string status);
    [HttpPost("webhook")]
    public async Task<IActionResult> UpdateStatus([FromBody] SalesOrderPaymentDTO paymentDetails)
    {
        var result = await _checkoutService.Webhook(paymentDetails.paymentIntent, paymentDetails.status);
        return CreateHttpResponse<SalesOrderPaymentDTO>(result);
    }
}
