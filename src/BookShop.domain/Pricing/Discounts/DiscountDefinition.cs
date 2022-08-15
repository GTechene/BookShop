using BookShop.domain.Pricing.Discounts.Targets;
using BookShop.domain.Pricing.Discounts.Types;

namespace BookShop.domain.Pricing.Discounts;

public class DiscountDefinition {

    public DiscountDefinition(DiscountType type, DiscountTarget target)
    {
        Type = type;
        Target = target;
    }
    private DiscountType Type { get; }

    private DiscountTarget Target { get; }
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