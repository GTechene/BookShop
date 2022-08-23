namespace BookShop.domain.Checkout.Payment;

public abstract record Payment {
    public record CardPayment(string Payment) : Payment;

    public static Payment ByCard(string payment) => new CardPayment(payment);
};