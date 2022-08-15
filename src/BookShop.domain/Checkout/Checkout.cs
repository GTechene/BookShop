using BookShop.domain.Prices;
using BookShop.domain.Pricing;

namespace BookShop.domain.Checkout;

public record Checkout(
    Cart Cart,
    Payment.Payment Payment,
    Price Price);