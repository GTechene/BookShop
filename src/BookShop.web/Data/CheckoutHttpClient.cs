namespace BookShop.web.Data; 

public class CheckoutHttpClient {
    private readonly HttpClient _httpClient;
    public CheckoutHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<string> Checkout(CheckoutRequest request)
    {
        var response = await _httpClient.PostAsync("/api/checkout", JsonContent.Create(request));

        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<string>();
    }

}

public record Address(
    string MainAddress,
    string AddionalAddress,
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
    Address ShippingAddress
) {
    public Address? BillingAddress { get; init; }
    public string? Email { get; init; }
};

public record CheckoutRequest(
    string[] Books,
    decimal Price,
    string Currency,
    Customer Customer,
    PaymentRequest Payment
);