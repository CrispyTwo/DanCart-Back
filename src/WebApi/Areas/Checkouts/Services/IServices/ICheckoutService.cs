using DanCart.WebApi.Areas.SalesOrders.DTOs;
using FluentResults;
using static DanCart.WebApi.Areas.Checkouts.Services.CheckoutService;

namespace DanCart.WebApi.Areas.Checkouts.Services.IServices;

public interface ICheckoutService
{
    Task<Result<CreatePaymentIntentResponse>> InitiateCheckout(string userId, SalesOrderCreateDTO dto);
    Task<Result> Webhook(string paymentIntent, string status);
}
