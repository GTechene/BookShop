using BookShop.domain.Checkout;
using BookShop.domain.Checkout.Payment;

namespace BookShop.infra;

public class PaymentProcessor : IProcessPayment
{
    public PaymentReceipt Process(Payment payment)
    {
        return PaymentReceipt.Success;
    }
}