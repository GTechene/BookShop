using BookShop.domain.Pricing.Discounts;

namespace BookShop.domain.Pricing.Prices;

public record Price(decimal Amount, string Currency)
{
    public static Price Zero(string currency) => new(0, currency);
    
    public static Price operator * (Price p, Percent percent)
    {
        return p with { Amount = p.Amount * percent };
    }
    
    public static Price operator + (Price p1, Price p2)
    {
        if (p1.Currency != p2.Currency)
        {
            throw new InvalidOperationException("Cannot add prices with difference currencies");
        }
        
        return p1 with { Amount = p1.Amount + p2.Amount };
    }
    
    public static bool operator <(Price p1, Price p2)
    {
        return p1.Amount < p2.Amount;
    }
    
    public static bool operator <=(Price p1, Price p2)
    {
        return p1.Amount <= p2.Amount;
    }
    
    public static bool operator >(Price p1, Price p2)
    {
        return p1.Amount > p2.Amount;
    }
    
    public static bool operator >=(Price p1, Price p2)
    {
        return p1.Amount >= p2.Amount;
    }

    
}