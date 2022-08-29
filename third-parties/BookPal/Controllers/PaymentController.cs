using System.Web;
using BookPal.Model;
using BookPal.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookPal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : Controller
{
    private readonly PaymentValidationService _paymentValidationService;
    private readonly PaymentSerializer _paymentSerializer;
    public PaymentController(
        PaymentValidationService paymentValidationService,
        PaymentSerializer paymentSerializer)
    {
        _paymentValidationService = paymentValidationService;
        _paymentSerializer = paymentSerializer;
    }

    public record PaymentValidationRequest(Card? Card, Price? Price, string? Payment);

    public record PaymentValidationResponse(PaymentValidationType Type, string? Payment, string? RedirectUrl); 
 
    [HttpPost]
    [Route("validation")]
    public async Task<IActionResult> ValidatePayment(PaymentValidationRequest request)
    {
        var (card, price, payment) = request;

        if (payment is not null)
        {
            var p = await _paymentSerializer.Deserialize(payment);
            if (!_paymentValidationService.Validate(p))
            {
                return BadRequest(new
                {
                    Message = "InvalidPayment"
                });
            }

            return Ok();
        }

        if (card is null || price is null)
        {
            return BadRequest();
        }
        
        var paymentValidation = _paymentValidationService.Validate(card, price);

        return Ok(ToResponse(paymentValidation));
    }
    
    public record ThreeDs2ValidationRequest(Card Card, Price Price, string User, string Password);

    
    [HttpPost]
    [Route("3ds2")]
    public async Task<PaymentValidationResponse> Validate3DS2(ThreeDs2ValidationRequest request)
    {
        var paymentValidation = _paymentValidationService.Validate3DSecure(
            request.Card,
            request.Price,
            (request.User, request.Password)
        );
        return await ToResponse(paymentValidation);;
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
    [Route("3ds1")]
    public IActionResult Validate3DS1(
        [FromForm]
        ThreeDs1ValidationRequest request
        )
    {
        return View(new Validate3DS1Model(
            new Card(request.CardNumber, request.CardOwner, request.CardExpirationDate, request.CardSecurityCode),
            new Price(request.Price, request.Currency),
            request.Redirect,
            _paymentValidationService
            ));
    }

    public record ThreeDs1ValidationExecutionRequest(
        string RedirectUrl,
        string UserName,
        string Password,
        string CardNumber,
        string CardOwner,
        DateTime CardExpirationDate,
        string CardSecurityCode,
        decimal Price,
        string Currency
    );

    [HttpPost]
    [Route("3ds1/execute")]
    public async Task<IActionResult> Execute3DS1Validation(
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
            var validation = _paymentValidationService.Validate3DSecure(card, new Price(request.Price, request.Currency), (request.UserName, request.Password));

            if (validation.Payment is null)
            {
                return Redirect(
                    request.RedirectUrl
                );    
            }
            
            var validationResponse = await ToResponse(validation);
            
            return Redirect(
                request.RedirectUrl + "&payment=" + HttpUtility.UrlEncode(validationResponse.Payment)
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
    
    private async Task<PaymentValidationResponse> ToResponse(PaymentValidation paymentValidation)
    {
        return new PaymentValidationResponse(
            paymentValidation.Type,
            paymentValidation.Payment != null ? await _paymentSerializer.Serialize(paymentValidation.Payment) : null,
            paymentValidation.RedirectionUrl
        );
    }
}

