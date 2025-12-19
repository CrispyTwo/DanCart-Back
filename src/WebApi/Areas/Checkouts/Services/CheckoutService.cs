using AutoMapper;
using DanCart.DataAccess.Extensions;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.WebApi.Areas.Checkouts.Services.IServices;
using DanCart.WebApi.Areas.SalesOrders.DTOs;
using DanCart.WebApi.Areas.SalesOrders.Services.IServices;
using DanCart.WebApi.Core;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace DanCart.WebApi.Areas.Checkouts.Services;

public class CheckoutService(IUnitOfWork _unitOfWork, IMapper _mapper, ISalesOrdersService _salesOrdersService) : ServiceBase, ICheckoutService
{
    public sealed record CreatePaymentIntentResponse(string clientSecret);
    public async Task<Result<CreatePaymentIntentResponse>> InitiateCheckout(string userId, SalesOrderCreateDTO dto)
    {
        var cart = await _unitOfWork.ShoppingCart.GetQuery().Where(x => x.UserId == userId).GetAllAsync();
        if (cart == null || !cart.Any())
        {
            return Result.Fail(
                new Error("Your cart is empty")
                    .WithMetadata(ErrorMetadata.Code, ErrorCode.UnprocessableEntity));
        }

        var result = await _salesOrdersService.CreateAsync(userId, dto, cart);
        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        _unitOfWork.ShoppingCart.RemoveRange(cart);

        long amountInCents = (long)(result.Value.Total * 100);
        var options = new PaymentIntentCreateOptions
        {
            Amount = amountInCents,
            Currency = "usd",

            Metadata = new Dictionary<string, string> { { "OrderId", result.Value.Id.ToString() }, { "Email", userId } }
        };

        var service = new PaymentIntentService();
        var intent = service.Create(options);

        var salesOrder = await _unitOfWork.SalesOrder.GetQuery(tracking: true).FirstAsync(x => x.Id == result.Value.Id);
        salesOrder.PaymentIntendId = intent.Id;
        await _unitOfWork.SaveAsync();

        return Result.Ok(new CreatePaymentIntentResponse(intent.ClientSecret));
    }

    public async Task<Result> Webhook(string paymentIntent, string status)
    {
        switch (status)
        {
            case "succeeded":
                return await _salesOrdersService.SucceedPayment(paymentIntent);

            default:
                return Result.Fail(new Error($"Unrecognized status: {status}")
                    .WithMetadata(ErrorMetadata.Code, ErrorCode.UnprocessableEntity));
        }
    }
}
