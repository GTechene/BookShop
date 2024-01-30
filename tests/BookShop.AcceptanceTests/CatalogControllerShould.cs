using BookShop.domain.Catalog;
using Diverse;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NFluent;
using NSubstitute;
using Xunit;
using Xunit.Abstractions;

namespace BookShop.AcceptanceTests;

public class CatalogControllerShould
{
    [Fact]
    public async Task List_all_books_when_called_on_GetCatalog()
    {
        var scenario = new CatalogListScenario();

        var api = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var metadataProvider = Substitute.For<IProvideBookMetadata>();
                var bookReferences = scenario.Books.Select(book => book.ToBookReference()).ToList();
                metadataProvider.Get().Returns(bookReferences);

                var inventoryProvider = Substitute.For<IProvideInventory>();
                var books = scenario.Books.Select(book => book.ToBook()).ToList();
                inventoryProvider.Get(Arg.Any<IEnumerable<BookReference>>())
                    .Returns(books);

                services.AddTransient<IProvideBookMetadata>(_ => metadataProvider);
                services.AddTransient<IProvideInventory>(_ => inventoryProvider);
            });
        });
        var client = api.CreateDefaultClient();
        var response = await client.GetAsync("api/Catalog?currency=EUR");
        Check.That(response.StatusCode).IsEqualTo(200);
    }

    [Fact]
    public async Task Return_2_pages_when_there_are_5_books_in_the_catalog_and_the_number_of_books_to_display_on_one_page_is_3()
    {
        var scenario = new CatalogListScenario()
            .WithNumberOfBooksPerPage(3)
            .WithRandomBooks(5);
    }

    public CatalogControllerShould(ITestOutputHelper output)
    {
        Fuzzer.Log = output.WriteLine;
    }
}