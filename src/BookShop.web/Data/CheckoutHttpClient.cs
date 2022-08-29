using BookShop.shared;

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