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

    public (Cart target, Cart remainingCart) ApplyTo(Cart cart)
    {
        return Target.GetTarget(cart);
    }

    public bool IsApplicable(Cart cart)
    {
        return Target.Matches(cart);
    }

    public Price ApplyDiscount(Price price)
    {
        return Type.Apply(price);
    }
}