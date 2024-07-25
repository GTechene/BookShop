using BookShop.AcceptanceTests.Simulators;
using sas.Api;
using sas.Configurations;
using sas.Simulators;

namespace BookShop.AcceptanceTests;

public class CatalogApi : BaseApi<Program>
{
    private CatalogApi(CatalogListScenario scenario, ISimulateBehaviour[] simulators, IEnrichConfiguration[] configurations) : base(scenario, simulators, configurations) {}

    public static CatalogApi CreateApi(CatalogListScenario scenario)
    {
        return new CatalogApi(scenario, [
            new BookAdvisorSimulator(),
            new InventorySimulator(),
            new MetadataSimulator()
        ], []);
    }

    public async Task<HttpResponseMessage> GetCatalog(string currency, int numberOfBooksPerPage = 5)
    {
        return await HttpClient.GetAsync($"api/Catalog?currency={currency}&pageNumber=1&numberOfItemsPerPage={numberOfBooksPerPage}");
    }
}