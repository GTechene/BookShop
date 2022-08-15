using BookShop.domain.Prices;

namespace BookShop.domain.Pricing.Discounts.Types;

public sealed class PercentageDiscountType : DiscountType {

    public PercentageDiscountType(Percent discountInPercent)
    {
        DiscountInPercent = discountInPercent;
    }
    public Percent DiscountInPercent { get; }

    public override Price Apply(Price price)
    {
        return price * DiscountInPercent.ComplementaryRatioValue();
    }

    public override string ToString()
    {
        return $"{DiscountInPercent}";
    }
}