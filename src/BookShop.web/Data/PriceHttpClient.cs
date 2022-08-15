namespace BookShop.web.Data;

public class PriceHttpClient
{
    private readonly HttpClient _httpClient;

    public PriceHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<PriceDetails?> GetCartPrice(IEnumerable<string> books)
    {
        return _httpClient.GetFromJsonAsync<PriceDetails>($"/api/Price{QueryString(books)}");
    }

    private static string QueryString(IEnumerable<string> books)
    {
        var booksQueryString = string.Join('&', books.Select(book => $"Books={book}"));

        var queryString = $"?Currency=EUR&{booksQueryString}";
        return queryString;
    }
}