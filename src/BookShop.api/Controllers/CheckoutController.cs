using BookShop.domain;
using BookShop.domain.Checkout;
using BookShop.domain.Checkout.Payment;
using BookShop.domain.Prices;
using BookShop.domain.Pricing;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CheckoutController : ControllerBase
{
    private readonly CheckoutService _checkoutService;

    public CheckoutController(CheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }
    
    [HttpPost]
    public CheckoutResponse ProcessCheckout(CheckoutRequest request)
    {
        var cart = request.Books.Select(ISBN.Parse)
            .Aggregate(Cart.Empty, (cart, isbn) => cart.Add(isbn));

        var checkout = new Checkout(cart, new Payment(), new Price(request.Price, request.Currency));

        var receipt = _checkoutService.ProcessCheckout(checkout);

        var booksByQuantity = receipt.Books.ToDictionary(
            tuple => tuple.Book.Reference.Id.ToString(),
            tuple => tuple.Quantity.Amount);
        
        var booksDetails = receipt.Books.ToDictionary(
            tuple => tuple.Book.Reference.Id.ToString(),
            tuple => new PurchasedBookResponse(tuple.Book.Reference.Id.ToString(), tuple.Book.Reference.Title));
        
        return new CheckoutResponse(
            receipt.Id.ToString(),
            booksByQuantity,
            booksDetails);
    }
}

public record PaymentRequest();

public record CheckoutRequest(
    string[] Books,
    decimal Price,
    string Currency,
    PaymentRequest Payment
);

public record PurchasedBookResponse(string ISBN, string Title);

public record CheckoutResponse(
    string ReceiptId,
    Dictionary<string, int> PurchasedBooks, // ISBN/Quantity
    Dictionary<string, PurchasedBookResponse> Books); 