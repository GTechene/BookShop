namespace BookShop.domain.Checkout.Payment;

public interface IProcessPayment {
    PaymentReceipt Process(Payment payment);
}