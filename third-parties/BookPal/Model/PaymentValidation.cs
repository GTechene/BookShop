namespace BookPal.Model;

public record PaymentValidation(PaymentValidationType Type) {
    public static PaymentValidation Validated(Payment payment) => new(PaymentValidationType.None)
    {
        Payment = payment
    };
    public static PaymentValidation Requires3Ds1(string url) => new(PaymentValidationType.With3DS1)
    {
        RedirectionUrl = url
    };
    
    public static PaymentValidation Requires3Ds2 => new(PaymentValidationType.With3DS2);
    
    public string? RedirectionUrl { get; init; }
    public Payment? Payment { get; init; }
}