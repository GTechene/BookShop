namespace BookShop.web.Data; 

public class CheckoutHttpClient {
    private readonly HttpClient _httpClient;
    public CheckoutHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CheckoutResponse> Checkout(CheckoutRequest request)
    {
        var response = await _httpClient.PostAsync("/api/checkout", JsonContent.Create(request));

        response.EnsureSuccessStatusCode();

        var checkoutResponse = await response.Content.ReadFromJsonAsync<CheckoutResponse>();

        if (checkoutResponse is null)
        {
            throw new InvalidDataException("No payload returned by checkout api");
        }
        
        return checkoutResponse;
    }
}

public record Address(
    string MainAddress,
    string? AdditionalAddress,
    string ZipCode,
    string Country
);

public record PaymentRequest(
    Card Card,
    string PaymentHash
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
    PaymentRequest Payment
);

public record CheckoutResponse(string ReceiptId);