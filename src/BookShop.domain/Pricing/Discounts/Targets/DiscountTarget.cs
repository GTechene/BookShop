namespace BookShop.domain.Pricing.Discounts.Targets;

public abstract class DiscountTarget
{
    public abstract bool Matches(Cart cart);
    public abstract (Cart target, Cart remaining) GetTarget(Cart cart);
}