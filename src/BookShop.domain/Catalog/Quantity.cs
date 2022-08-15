namespace BookShop.domain.Catalog;

public record Quantity(int Amount)
{
    public static implicit operator Quantity(int amount)
    {
        return new Quantity(amount);
    }
    
    public static bool operator <(Quantity q1, Quantity q2)
    {
        return q1.Amount < q2.Amount;
    }
    
    public static bool operator <=(Quantity q1, Quantity q2)
    {
        return q1.Amount <= q2.Amount;
    }
    
    public static bool operator >(Quantity q1, Quantity q2)
    {
        return q1.Amount > q2.Amount;
    }
    
    public static bool operator >=(Quantity q1, Quantity q2)
    {
        return q1.Amount >= q2.Amount;
    }
    
    public static Quantity operator +(Quantity q1, Quantity q2)
    {
        return new Quantity(Amount: q1.Amount + q2.Amount);
    }
    public static Quantity operator -(Quantity q1, Quantity q2)
    {
        return new Quantity(Amount: q1.Amount - q2.Amount);
    }
}