using BookShop.shared;
using Diverse;
using NFluent;
using Xunit;
using Xunit.Abstractions;

namespace BookShop.AcceptanceTests;

public class CatalogControllerShould
{
    [Fact]
    public async Task List_all_books_when_called_on_GetCatalog()
    {
        var scenario = new CatalogListScenario();
        var api = CatalogApi.CreateApi(scenario);

        var response = await api.GetCatalog("EUR");

        Check.That(response).IsOk<CatalogResponse>()
            .WhichPayload(catalogResponse =>
            {
                Check.That(catalogResponse).IsNotNull();
                Check.That(catalogResponse!.Books).HasSize(scenario.Books.Length);
                Check.That(catalogResponse.TotalNumberOfPages).IsEqualTo(1);
            });
    }

    [Fact]
    public async Task Return_2_pages_when_there_are_5_books_in_the_catalog_and_the_number_of_books_to_display_on_one_page_is_3()
    {
        var scenario = new CatalogListScenario()
            .WithNumberOfBooksPerPage(3)
            .WithRandomBooks(5);

        var api = CatalogApi.CreateApi(scenario);

        var response = await api.GetCatalog("EUR", 3);

        Check.That(response).IsOk<CatalogResponse>()
            .WhichPayload(catalogResponse =>
            {
                Check.That(catalogResponse).IsNotNull();
                Check.That(catalogResponse!.Books).HasSize(3);
                Check.That(catalogResponse.TotalNumberOfPages).IsEqualTo(2);
            });
    }

    public CatalogControllerShould(ITestOutputHelper output)
    {
        Fuzzer.Log = output.WriteLine;
    }
}