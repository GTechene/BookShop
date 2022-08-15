namespace BookShop.domain.Checkout.Payment;

public class PaymentProcessFailed : Exception {
    public PaymentProcessFailed(string failureReason) : base($"Could not process payment : {failureReason}") {}
}