namespace BookShop.domain.Checkout.Payment;

public abstract record PaymentReceipt {

    public static PaymentReceipt Success => new SuccessReceipt();

    public static PaymentReceipt Failure(string reason)
    {
        return new FailureReceipt(reason);
    }
    public record SuccessReceipt : PaymentReceipt;

    public record FailureReceipt(string Reason) : PaymentReceipt;
}