using System.Web;

namespace BookShop.web.Data;

public class PriceHttpClient
{
    private readonly HttpClient httpClient;

    public PriceHttpClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public Task<PriceDetails?> GetCartPrice(IEnumerable<string> books)
    {
        return httpClient.GetFromJsonAsync<PriceDetails>($"/api/Price{QueryString(books)}");
    }

    private static string QueryString(IEnumerable<string> books)
    {
        var booksQueryString = string.Join('&', books.Select(book => $"Books={book}"));

        var queryString = $"?Currency=EUR&{booksQueryString}";
        return queryString;
    }
}

public record PriceDetails(Price Total, string[] Discounts); 

