using BookShop.domain;
using BookShop.domain.Checkout;
using BookShop.domain.Checkout.Payment;
using BookShop.domain.Pricing;
using BookShop.shared;
using Microsoft.AspNetCore.Mvc;

using Price = BookShop.domain.Prices.Price;

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
    public async Task<CheckoutResponse> ProcessCheckout(CheckoutRequest request)
    {
        var cart = request.Books.Select(ISBN.Parse)
            .Aggregate(Cart.Empty, func: (cart, isbn) => cart.Add(isbn));

        var checkout = new Checkout(cart, 
            Payment.ByCard(request.Payment), new Price(request.Price, request.Currency));

        var receipt = await _checkoutService.ProcessCheckout(checkout);

        return new CheckoutResponse(receipt.Id.ToString());
    }
}

