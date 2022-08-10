namespace BookShop.web.Data;

public record Price(decimal Amount, string Currency);

public class CatalogHttpClient
{
    private readonly HttpClient _httpClient;

    public CatalogHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<Catalog?> GetCatalog(int requestedPage = 1) =>
        _httpClient.GetFromJsonAsync<Catalog>($"/api/Catalog?Currency=EUR&PageNumber={requestedPage}");

}

public record Catalog(Book[] Books, int TotalNumberOfPages);

public record Book(string ISBN, string Title, string Author, string PictureUrl, int Quantity, Price UnitPrice);