namespace BookShop.domain.Catalog;

public record Quantity(int Amount)
{
    public static implicit operator Quantity(int amount)
    {
        return new Quantity(amount);
    }
}