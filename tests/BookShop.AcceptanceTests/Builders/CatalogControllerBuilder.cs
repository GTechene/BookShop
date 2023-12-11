using BookShop.api.Controllers;
using BookShop.domain;
using BookShop.domain.Catalog;
using BookShop.infra;
using Diverse;
using NSubstitute;

namespace BookShop.AcceptanceTests.Builders;

public class CatalogControllerBuilder
{
    private readonly IFuzz _fuzzer = new Fuzzer();
    private Book[] _booksInCatalog = Array.Empty<Book>();

    public CatalogControllerBuilder WithRandomBooks(int numberOfBooksToGenerate)
    {
        _booksInCatalog = Enumerable
            .Range(0, numberOfBooksToGenerate)
            .Select(_ => {
                var randomIsbn = new ISBN.ISBN10(_fuzzer.GenerateInteger(1, 100), _fuzzer.GenerateInteger(1, 10000), _fuzzer.GenerateInteger(1, 1000), _fuzzer.GenerateInteger(1, 10));

                var title = _fuzzer.GenerateSentence(6);
                var firstName = _fuzzer.GenerateFirstName();
                var lastName = _fuzzer.GenerateLastName(firstName);
                var authorName = $"{firstName} {lastName}";
                var numberOfPages = _fuzzer.GenerateInteger(10, 1500);
                var pictureUrl = new Uri(_fuzzer.GenerateStringFromPattern("http://picture-url-for-tests/xxxxxxx.jpg"));
                var quantityInStock = _fuzzer.GenerateInteger(1, 100);

                return new Book(new BookReference(randomIsbn, title, authorName, numberOfPages, pictureUrl), quantityInStock);
            })
            .ToArray();

        return this;
    }

    public CatalogController Build()
    {
        var bookMetadataProvider = StubBookMetadataProvider();
        var inventoryProvider = StubInventoryProvider();

        var catalogProvider = new CatalogService(bookMetadataProvider, inventoryProvider);

        var bookPriceProvider = new BookPriceRepository();
        var bookAdvisorHttpClient = StubBookAdvisorHttpClient();

        return new CatalogController(catalogProvider, bookPriceProvider, bookMetadataProvider, bookAdvisorHttpClient);
    }

    private IProvideBookMetadata StubBookMetadataProvider()
    {
        var bookMetadataProvider = Substitute.For<IProvideBookMetadata>();

        foreach (var book in _booksInCatalog)
        {
            bookMetadataProvider.Get(book.Reference.Id)
                .Returns(book.Reference);
        }

        bookMetadataProvider.Get()
            .Returns(_booksInCatalog.Select(book => book.Reference).ToList());

        return bookMetadataProvider;
    }

    private IProvideInventory StubInventoryProvider()
    {
        var inventoryProvider = Substitute.For<IProvideInventory>();
        inventoryProvider.Get(Arg.Any<IEnumerable<BookReference>>())
            .Returns(callInfo =>
            {
                var requestedBookReferences = callInfo.Arg<IEnumerable<BookReference>>();

                return _booksInCatalog.Where(book => requestedBookReferences.Contains(book.Reference));
            });

        return inventoryProvider;
    }

    private BookAdvisorHttpClient StubBookAdvisorHttpClient()
    {
        return new BookAdvisorHttpClient(new HttpClient(new MockBookAdvisorHttpHandler(_fuzzer))
        {
            BaseAddress = new Uri("http://fake-base-address-for-tests")
        });
    }
}