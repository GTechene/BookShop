using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
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
