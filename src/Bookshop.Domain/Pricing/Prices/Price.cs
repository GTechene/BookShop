using BookShop.domain.Pricing.Discounts;

namespace BookShop.domain.Pricing.Prices;

public record Price(decimal Amount) : IComparable<Price>
{
    public static readonly Price Zero = new(0);
    
    public static implicit operator Price(decimal price)
    {
        return new Price(price);
    }
    
    public static Price operator * (Price p, Percent percent)
    {
        return new Price(p.Amount * percent);
    }
    
    public static Price operator + (Price p1, Price p2)
    {
        return new Price(p1.Amount + p2.Amount);
    }

    public int CompareTo(Price? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Amount.CompareTo(other.Amount);
    }
}