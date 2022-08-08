namespace BookShop.domain.Pricing.Discounts.Targets;

public sealed class DistinctBooksTitleTarget : DiscountTarget
{
    private readonly int count;

    public DistinctBooksTitleTarget(int count)
    {
        this.count = count;
    }
    
    public override bool Matches(Cart cart)
    {
        return cart.Distinct().Count() >= count;
    }

    public override (Cart target, Cart remaining) GetTarget(Cart cart)
    {
        var titles = cart.Distinct().Take(count).ToArray();
        var target = Cart.Empty.Add(titles);
        var remaining = cart.Remove(titles);
        return (target, remaining);
    }

    public override string ToString()
    {
        return $"{count} distinct book titles";
    }
}