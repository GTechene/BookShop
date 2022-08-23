namespace BookShop.domain.Checkout.Payment;

public interface IProcessPayment {
    Task<PaymentReceipt> Process(Payment payment);
}