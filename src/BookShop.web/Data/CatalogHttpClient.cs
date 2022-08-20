namespace BookShop.web.Data;

public class CatalogHttpClient {
    private readonly HttpClient _httpClient;

    public CatalogHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<Catalog?> GetCatalog(int requestedPage = 1)
    {
        return _httpClient.GetFromJsonAsync<Catalog>($"/api/Catalog?Currency=EUR&PageNumber={requestedPage}");
    }
    
    public Task<BookReference?> GetBookReference(string isbn)
    {
        return _httpClient.GetFromJsonAsync<BookReference>($"/api/Catalog/{isbn}");
    }

}