﻿namespace BookShop.web.Data;

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