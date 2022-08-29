namespace BookShop.shared;

public record Card(
    string Number,
    DateTime ExpirationDate,
    string SecurityCode,
    string OwnerName
);
    
public record PaymentRequest(
    Card Card,
    string PaymentHash
);
    
public record Address(
    string MainAddress,
    string? AdditionalAddress,
    string ZipCode,
    string Country
);

public record Customer(
    string FirstName,
    string LastName,
    string UserName,
    string? Email,
    Address BillingAddress,
    Address? ShippingAddress
);

public record CheckoutRequest(
    string[] Books,
    decimal Price,
    string Currency,
    Customer Customer,
    string Payment
);