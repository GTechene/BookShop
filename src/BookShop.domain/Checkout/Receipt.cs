using BookShop.domain.Catalog;
using BookShop.domain.Prices;

namespace BookShop.domain.Checkout;

public record Receipt(
    ReceiptId Id,
    IReadOnlyCollection<(Book Book, Quantity Quantity)> Books, 
    Price Price);