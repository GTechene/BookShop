using Microsoft.AspNetCore.Mvc;

namespace BookPal.Controllers;

[ApiController]
[Route("payments")]
public class PaymentController : ControllerBase
{
    [HttpGet]
    [Route("methods")]
    public IEnumerable<PaymentMethod> GetPaymentMethods()
    {
        var allPaymentMethods = Enum.GetValues<PaymentMethod>();
        return allPaymentMethods;
    }
}

public enum PaymentMethod
{
    Visa,
    Mastercard,
    AmericanExpress,
    ChinaUnionPay,
    Paypal,
    TravelersCheque,
    PasseCulture
}