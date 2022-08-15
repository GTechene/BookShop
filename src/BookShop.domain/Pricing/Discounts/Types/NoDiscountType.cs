using BookShop.domain.Prices;

namespace BookShop.domain.Pricing.Discounts.Types;

public sealed class NoDiscountType : DiscountType {
    public static readonly NoDiscountType Instance = new();

    private NoDiscountType() {}

    public override Price Apply(Price price)
    {
        return price;
    }

    public override string ToString()
    {
        return "NO DISCOUNT";
    }
}