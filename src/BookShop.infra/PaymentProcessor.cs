using BookShop.domain.Checkout.Payment;

namespace BookShop.infra;

public class PaymentProcessor : IProcessPayment {
    public PaymentReceipt Process(Payment payment)
    {
        // TODO : we should do something smarter here.
        return PaymentReceipt.Success;
    }
}