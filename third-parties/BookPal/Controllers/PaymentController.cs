using System.Web;
using BookPal.Model;
using BookPal.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookPal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : Controller
{
    private readonly CardValidationService _cardValidationService;
    public PaymentController(
        CardValidationService cardValidationService)
    {
        _cardValidationService = cardValidationService;
    }
    
    [HttpPost]
    [Route("card/validation")]
    public CardValidation GetCardValidation(Card card)
    {
        return _cardValidationService.GetValidationFor(card);
    }
    

    public record ThreeDs2ValidationRequest(Card Card, string User, string Password);
    
    [HttpPost]
    [Route("card/3ds2")]
    public CardValidation Validate3DS2(ThreeDs2ValidationRequest request)
    {
        return _cardValidationService.Get3DSValidationFor(
            request.Card,
            (request.User, request.Password)
        );
    }
    
    
    public record ThreeDs1ValidationRequest(
        string CardNumber,
        string CardOwner,
        string CardSecurityCode,
        DateTime CardExpirationDate,
        
        decimal Price,
        string Currency,
        
        string Redirect
    );
    
    [HttpPost]
    [Route("card/3ds1")]
    public IActionResult Validate3DS1(
        [FromForm]
        ThreeDs1ValidationRequest request
        )
    {
        return View(new Validate3DS1Model(
            new Card(request.CardNumber, request.CardOwner, request.CardExpirationDate, request.CardSecurityCode),
            new Price(request.Price, request.Currency),
            request.Redirect,
            _cardValidationService
            ));
    }
    
    public record ThreeDs1ValidationExecutionRequest(
        string RedirectUrl,
        string UserName,
        string Password,
        string CardNumber,
        string CardOwner,
        DateTime CardExpirationDate,
        string CardSecurityCode
    );
    
    [HttpPost]
    [Route("card/3ds1/execute")]
    public IActionResult Execute3DS1Validation(
        [FromForm]
        ThreeDs1ValidationExecutionRequest request
    )
    {
        var card = new Card(
            request.CardNumber,
            request.CardOwner,
            request.CardExpirationDate,
            request.CardSecurityCode
        );

        try
        {
            var validation = _cardValidationService.Get3DSValidationFor(card, (request.UserName, request.Password));
            
            return Redirect(
                request.RedirectUrl + "&paymenthash=" + HttpUtility.UrlEncode(validation.PaymentHash)
            );

        }
        catch (ValidationUsing3DS2Rejected)
        {
            return Redirect(
                request.RedirectUrl
            );
        }
    }
    
    [HttpGet]
    [Route("methods")]
    public IEnumerable<PaymentMethod> GetPaymentMethods()
    {
        var allPaymentMethods = Enum.GetValues<PaymentMethod>();
        return allPaymentMethods;
    }
}

