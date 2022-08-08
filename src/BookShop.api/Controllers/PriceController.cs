using BookShop.domain;
using BookShop.domain.Pricing;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PriceController : ControllerBase
{
    private readonly CartPricer pricer;

    public PriceController(CartPricer pricer)
    {
        this.pricer = pricer;
    }
    
    [HttpGet]
    public PriceResponse GetPrice([FromQuery]PriceRequest request)
    {
        var cart = Cart.Empty
            .Add(request.Books.Select(ISBN.Parse).ToArray());

        var price = pricer.ComputePrice(cart);

        return new PriceResponse(price.Amount);
    }
}

public record PriceRequest(string[] Books);
public record PriceResponse(decimal Price);