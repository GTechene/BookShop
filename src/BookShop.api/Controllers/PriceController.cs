﻿using BookShop.domain;
using BookShop.domain.Prices;
using BookShop.domain.Pricing;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PriceController : ControllerBase
{
    private readonly CartPricer _pricer;

    public PriceController(CartPricer pricer)
    {
        _pricer = pricer;
    }
    
    [HttpGet]
    public PriceResponse GetPrice([FromQuery]PriceRequest request)
    {
        var cart = Cart.Empty
            .Add(request.Books.Select(ISBN.Parse).ToArray());

        var (price, discounts) = _pricer.ComputePrice(cart, request.Currency);

        return new PriceResponse(
            price,
            discounts.Select(discount => $"{discount.Type} discount applied").ToArray()
            );
    }
}

public record PriceRequest(string[] Books, string Currency);
public record PriceResponse(Price Total, string[] Discounts);