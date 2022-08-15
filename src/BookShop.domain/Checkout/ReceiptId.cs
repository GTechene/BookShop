namespace BookShop.domain.Checkout;

public record ReceiptId(Guid Id)
{
    public static ReceiptId Generate() => new(Guid.NewGuid());

    public override string ToString()
    {
        return Id.ToString();
    }
};