namespace BookShop.domain.Pricing.Discounts;

public class Percent
{
    private readonly decimal amount;

    private Percent(decimal amount)
    {
        this.amount = amount;
    }
        
    public static implicit operator Percent(decimal amount)
    {
        return new Percent(amount);
    }
    
    public static implicit operator decimal(Percent percent)
    {
        return percent.amount;
    }

    private decimal RatioValue()
    {
        return amount / 100;
    }

    public Percent ComplementaryRatioValue()
    {
        return 1 - RatioValue();
    }
    
    

    public override string ToString()
    {
        return $"{amount}%";
    }
}