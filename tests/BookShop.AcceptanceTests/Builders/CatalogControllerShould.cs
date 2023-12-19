using BookShop.shared;
using Diverse;
using NFluent;
using Xunit;
using Xunit.Abstractions;

namespace BookShop.AcceptanceTests.Builders;

public class CatalogControllerShould
{
    [Fact]
    public async Task List_all_books_when_called_on_GetCatalog()
    {
        var fuzzer = new Fuzzer();

        var books = new BookSpecification[]
        {
            new(fuzzer),
            new(fuzzer),
            new(fuzzer)
        };

        var controller = new CatalogControllerBuilder()
            .WithBooks(books)
            .Build();

        var catalogResponse = await controller.GetCatalog("EUR");
        
        Check.That(catalogResponse.Books).HasSize(books.Length);

        // This because of the sloppy implementation of our BookPriceRepository :)
        var uniqueBookPrice = new Price(8m, "EUR");

        var expectedResponse = books.Select(book => new BookResponse(book.Isbn.ToString(), book.Title, book.Author, book.NumberOfPages, new RatingsResponse(book.AverageRating, book.NumberOfRatings), book.PictureUrl.ToString(), book.Quantity.Amount, uniqueBookPrice));
        Check.That(catalogResponse.Books).IsEquivalentTo(expectedResponse);
    }

    public CatalogControllerShould(ITestOutputHelper output)
    {
        Fuzzer.Log = output.WriteLine;
    }
}