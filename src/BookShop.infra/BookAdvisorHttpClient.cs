using System.Net.Http.Json;
using BookShop.domain;
using BookShop.shared;

namespace BookShop.infra;

public class BookAdvisorHttpClient
{
    private readonly HttpClient _httpClient;

    public BookAdvisorHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RatingsResponse?> GetRatings(ISBN isbn)
    {
        return await _httpClient.GetFromJsonAsync<RatingsResponse>($"/reviews/ratings/{isbn}");
    }
}
