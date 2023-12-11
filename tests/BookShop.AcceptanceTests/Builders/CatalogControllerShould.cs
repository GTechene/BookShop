using Diverse;
using NFluent;
using Xunit;
using Xunit.Abstractions;

namespace BookShop.AcceptanceTests.Builders;

public class CatalogControllerShould
{
    public CatalogControllerShould(ITestOutputHelper output)
    {
        Fuzzer.Log = output.WriteLine;
    }

    [Fact]
    public async Task List_all_books_when_called_on_GetCatalog()
    {
        const int numberOfBooksInCatalog = 3;

        var controller = new CatalogControllerBuilder()
            .WithRandomBooks(numberOfBooksInCatalog)
            .Build();

        var catalogResponse = await controller.GetCatalog("EUR");

        Check.That(catalogResponse.Books).HasSize(numberOfBooksInCatalog);
    }
}