namespace BookPal.Model;

public record CardValidation(CardValidationType Type) {
    public static CardValidation NoValidationRequired(string paymentHash) => new(CardValidationType.None)
    {
        PaymentHash = paymentHash
    };
    public static CardValidation Requires3Ds1(string url) => new(CardValidationType.With3DS1)
    {
        WebSiteUrl = url
    };
    
    public static CardValidation Requires3Ds2 => new(CardValidationType.With3DS2);
    
    public string? WebSiteUrl { get; init; }
    public string? PaymentHash { get; init; }
}