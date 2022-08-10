using BookShop.domain.Pricing.Prices;

namespace BookShop.domain.Pricing.Discounts.Types;

public sealed class PercentageDiscountType : DiscountType
{
    public Percent DiscountInPercent { get; }

    public PercentageDiscountType(Percent discountInPercent)
    {
        DiscountInPercent = discountInPercent;
    }
    
    public override Price Apply(Price price)
    {
        return price * DiscountInPercent.ComplementaryRatioValue();
    }

    public override string ToString()
    {
        return $"{DiscountInPercent}";
    }

}