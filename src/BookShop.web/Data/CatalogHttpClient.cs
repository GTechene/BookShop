namespace BookShop.web.Data;

public record Price(decimal Amount, string Currency);

public class CatalogHttpClient
{
    private readonly HttpClient httpClient;

    public CatalogHttpClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public Task<Catalog?> GetCatalog() =>
        httpClient.GetFromJsonAsync<Catalog>("/api/Catalog?Currency=EUR");

}

public record Catalog(Book[] Books);

public record Book(string ISBN, string Title, string Author, string PictureUrl, int Quantity, Price UnitPrice);