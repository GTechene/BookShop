using BookShop.domain.Pricing.Prices;

namespace BookShop.domain.Pricing.Discounts.Types;

public abstract class DiscountType
{
    public abstract Price Apply(Price price);
}