namespace BookShop.domain.Checkout.Payment;

internal class PaymentProcessFailed : Exception
{
    public PaymentProcessFailed(string failureReason) : base(failureReason) { }
}