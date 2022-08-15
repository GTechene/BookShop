using BookShop.domain.Prices;

namespace BookShop.domain.Checkout;

public interface ILogTransaction {
    void Add(ReceiptId id, IEnumerable<ISBN> books, Price checkoutPrice);
}