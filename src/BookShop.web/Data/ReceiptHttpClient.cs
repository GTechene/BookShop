using BookShop.shared;

namespace BookShop.web.Data; 

public class ReceiptHttpClient {
    private readonly HttpClient _httpClient;

    public ReceiptHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ReceiptResponse?> GetReceipt(string receiptId)
    {
        return _httpClient.GetFromJsonAsync<ReceiptResponse>($"/api/Receipt/{receiptId}");
    }
}