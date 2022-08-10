namespace BookShop.domain.Pricing.Discounts.Targets;

public sealed class DistinctBooksTitleTarget : DiscountTarget
{
    private readonly int _count;

    public DistinctBooksTitleTarget(int count)
    {
        _count = count;
    }
    
    public override bool Matches(Cart cart)
    {
        return cart.Distinct().Count() >= _count;
    }

    public override (Cart target, Cart remaining) GetTarget(Cart cart)
    {
        var titles = cart.Distinct().Take(_count).ToArray();
        var target = Cart.Empty.Add(titles);
        var remaining = cart.Remove(titles);
        return (target, remaining);
    }

    public override string ToString()
    {
        return $"{_count} distinct book titles";
    }
}