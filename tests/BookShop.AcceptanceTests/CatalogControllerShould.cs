using System.Net;
using System.Net.Http.Json;
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
        var api = new CatalogApi(scenario);

        var response = await api.GetCatalog("EUR");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var catalogResponse = await response.Content.ReadFromJsonAsync<CatalogResponse>();
        Check.That(catalogResponse).IsNotNull();
        Check.That(catalogResponse!.Books).HasSize(scenario.Books.Length);
        Check.That(catalogResponse.TotalNumberOfPages).IsEqualTo(1);
    }

    [Fact]
    public async Task Return_2_pages_when_there_are_5_books_in_the_catalog_and_the_number_of_books_to_display_on_one_page_is_3()
    {
        var scenario = new CatalogListScenario()
            .WithNumberOfBooksPerPage(3)
            .WithRandomBooks(5);

        var api = new CatalogApi(scenario);

        var response = await api.GetCatalog("EUR");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var catalogResponse = await response.Content.ReadFromJsonAsync<CatalogResponse>();
        Check.That(catalogResponse).IsNotNull();
        Check.That(catalogResponse!.TotalNumberOfPages).IsEqualTo(2);
        Check.That(catalogResponse.Books).HasSize(3);
    }

    public CatalogControllerShould(ITestOutputHelper output)
    {
        Fuzzer.Log = output.WriteLine;
    }
}