namespace BookShop.domain.Pricing.Discounts.Targets;

public sealed class AllBooksTarget : DiscountTarget
{
    public static readonly AllBooksTarget Instance = new();

    private AllBooksTarget() { }
    
    public override bool Matches(Cart cart)
    {
        return true;
    }

    public override (Cart target, Cart remaining) GetTarget(Cart cart)
    {
        return (cart, Cart.Empty);
    }

    public override string ToString()
    {
        return "Any books";
    }
}