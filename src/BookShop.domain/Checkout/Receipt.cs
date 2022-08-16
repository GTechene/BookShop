using BookShop.domain.Catalog;
using BookShop.domain.Prices;

namespace BookShop.domain.Checkout;

public record Receipt(
    ReceiptId Id,
    IReadOnlyCollection<(BookReference Book, Quantity Quantity)> Books,
    Price Price);