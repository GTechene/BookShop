using BookShop.AcceptanceTests.Simulators;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace BookShop.AcceptanceTests;

public class CatalogApi
{
    private readonly CatalogListScenario _scenario;
    private readonly HttpClient _httpClient;

    public CatalogApi(CatalogListScenario scenario)
    {
        _scenario = scenario;
        var api = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services => {
                var metadataProvider = new MetadataSimulator(scenario);
                metadataProvider.Register(services);

                var inventoryProvider = new InventorySimulator(scenario);
                inventoryProvider.Register(services);

                BookAdvisorSimulator.Register(services);
            });
        });

        _httpClient = api.CreateDefaultClient();
    }

    public async Task<HttpResponseMessage> GetCatalog(string currency)
    {
        return await _httpClient.GetAsync($"api/Catalog?currency={currency}&pageNumber=1&numberOfItemsPerPage={_scenario.NumberOfBooksPerPage}");
    }
}