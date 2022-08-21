using BookShop.domain;
using BookShop.domain.Checkout;
using BookShop.domain.Checkout.Payment;
using BookShop.domain.Prices;
using BookShop.domain.Pricing;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckoutController : ControllerBase {
    private readonly CheckoutService _checkoutService;

    public CheckoutController(CheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    [HttpPost]
    public CheckoutResponse ProcessCheckout(CheckoutRequest request)
    {
        var cart = request.Books.Select(ISBN.Parse)
            .Aggregate(Cart.Empty, func: (cart, isbn) => cart.Add(isbn));

        var checkout = new Checkout(cart, 
            new Payment(), new Price(request.Price, request.Currency));

        var receipt = _checkoutService.ProcessCheckout(checkout);

        return new CheckoutResponse(
            receipt.Id.ToString());
    }

    public record Card(
        string Number,
        DateTime ExpirationDate,
        string SecurityCode,
        string OwnerName
    );
    
    public record PaymentRequest(
        Card Card,
        string PaymentHash
    );
    
    public record Address(
        string MainAddress,
        string? AdditionalAddress,
        string ZipCode,
        string Country
    );

    public record Customer(
        string FirstName,
        string LastName,
        string UserName,
        string? Email,
        Address BillingAddress,
        Address? ShippingAddress
    );

    public record CheckoutRequest(
        string[] Books,
        decimal Price,
        string Currency,
        Customer Customer,
        PaymentRequest Payment
    );

    public record CheckoutResponse(
        string ReceiptId);
}

