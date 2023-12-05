using BookShop.domain.Checkout;

namespace BookShop.domain.Receipt;

public interface IProvideReceiptDetails
{
    ReceiptDetails Get(ReceiptId id);
}