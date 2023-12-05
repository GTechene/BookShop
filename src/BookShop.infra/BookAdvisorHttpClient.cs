using System.Net;
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

    public async Task<RatingsResponse> GetRatings(ISBN isbn)
    {
        var response = await _httpClient.GetAsync($"/reviews/ratings/{isbn}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new RatingsResponse(0m, 0);
        }

        return await response.Content.ReadFromJsonAsync<RatingsResponse>();
    }
}
