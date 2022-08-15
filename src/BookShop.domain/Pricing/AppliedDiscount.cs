using System.Collections;
using System.Collections.Immutable;
using BookShop.domain.Prices;
using BookShop.domain.Pricing.Discounts.Types;

namespace BookShop.domain.Pricing;

public record AppliedDiscount(Cart Cart, DiscountType Type)
{
    public Price Apply(Price price)
    {
        return Type.Apply(price);
    }
}

public class AppliedDiscounts : IEnumerable<AppliedDiscount>
{
    private ImmutableList<AppliedDiscount> AppliedDiscountsList { get; init; } = ImmutableList<AppliedDiscount>.Empty;

    public static readonly AppliedDiscounts Empty = new();

    public AppliedDiscounts Add(AppliedDiscount appliedDiscount)
    {
        return new AppliedDiscounts
        {
            AppliedDiscountsList = AppliedDiscountsList.Add(appliedDiscount)
        };
    }
    
    public AppliedDiscounts Append(AppliedDiscounts appliedDiscounts)
    {
        return new AppliedDiscounts
        {
            AppliedDiscountsList = appliedDiscounts.Aggregate(AppliedDiscountsList, (discounts, discount) => discounts.Add(discount))
        };
    }
    
    public IEnumerator<AppliedDiscount> GetEnumerator()
    {
        return AppliedDiscountsList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
