namespace BookShop.domain.Checkout.Payment;

public abstract record PaymentReceipt
{
    public record SuccessReceipt : PaymentReceipt;

    public static PaymentReceipt Success => new SuccessReceipt();
    
    public record FailureReceipt(string Reason) : PaymentReceipt;

    public static PaymentReceipt Failure(string reason) => new FailureReceipt(reason);
}