namespace BookShop.domain.Pricing.Discounts;

public class Percent {
    private readonly decimal _amount;

    private Percent(decimal amount)
    {
        _amount = amount;
    }

    public static implicit operator Percent(decimal amount)
    {
        return new Percent(amount);
    }

    public static implicit operator decimal(Percent percent)
    {
        return percent._amount;
    }

    private decimal RatioValue()
    {
        return _amount / 100;
    }

    public Percent ComplementaryRatioValue()
    {
        return 1 - RatioValue();
    }



    public override string ToString()
    {
        return $"{_amount}%";
    }
}