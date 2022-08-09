using BookShop.domain.Pricing.Discounts.Targets;
using BookShop.domain.Pricing.Discounts.Types;
using BookShop.domain.Pricing.Prices;

namespace BookShop.domain.Pricing.Discounts;

public class DiscountDefinition
{
    private DiscountType Type { get; }

    private DiscountTarget Target { get; }
    
    public DiscountDefinition(DiscountType type, DiscountTarget target)
    {
        Type = type;
        Target = target;
    }
    public override string ToString()
    {
        return $"Discount of {Type} for {Target}";
    }

    public (AppliedDiscount, Cart remainingCart) ApplyTo(Cart cart)
    {
        var (target, remainingCart) = Target.GetTarget(cart);
        return (new AppliedDiscount(target, Type), remainingCart);
    }

    public bool IsApplicable(Cart cart)
    {
        return Target.Matches(cart);
    }
}