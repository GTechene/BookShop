using BookShop.shared;

namespace BookShop.web.Data;

public class CatalogHttpClient {
    private readonly HttpClient _httpClient;

    public CatalogHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<CatalogResponse?> GetCatalog(int requestedPage = 1)
    {
        return _httpClient.GetFromJsonAsync<CatalogResponse>($"/api/Catalog?Currency=EUR&PageNumber={requestedPage}");
    }
    
    public Task<BookReferenceResponse?> GetBookReference(string isbn)
    {
        return _httpClient.GetFromJsonAsync<BookReferenceResponse>($"/api/Catalog/{isbn}");
    }

}