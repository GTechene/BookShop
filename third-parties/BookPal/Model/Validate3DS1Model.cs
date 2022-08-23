using BookPal.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookPal.Model;

public record Price(decimal Amount, string Currency);

public class Validate3DS1Model {

    public Card Card { get; }
    public Price Price { get; }
    private readonly PaymentValidationService _paymentValidationService;
    
    public string RedirectUrl { get; }
    public Validate3DS1Model(
        Card card, Price price, 
        string redirectUrl, 
        PaymentValidationService paymentValidationService)
    {
        Card = card;
        Price = price;
        _paymentValidationService = paymentValidationService;
        RedirectUrl = redirectUrl;
    }
    
    [BindProperty]
    public string UserName { get; set; } = null!;
    
    [BindProperty]
    public string Password { get; set; } = null!;
} ;