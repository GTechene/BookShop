using BookShop.domain.Prices;
using BookShop.domain.Receipt;

namespace BookShop.domain.Checkout;

public interface IManageTransactions
{
    void Add(ReceiptId id, IEnumerable<ISBN> books, Price checkoutPrice);
    Transaction? Find(ReceiptId id);
}