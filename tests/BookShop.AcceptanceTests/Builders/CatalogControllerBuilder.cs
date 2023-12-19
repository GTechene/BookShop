using BookShop.api.Controllers;
using BookShop.domain.Catalog;
using BookShop.infra;
using NSubstitute;

namespace BookShop.AcceptanceTests.Builders;

public class CatalogControllerBuilder
{
    private BookSpecification[] _booksInCatalog = Array.Empty<BookSpecification>();

    public CatalogControllerBuilder WithBooks(params BookSpecification[] books)
    {
        _booksInCatalog = books;
        return this;
    }

    public CatalogController Build()
    {
        var catalogServiceBuilder = new CatalogServiceBuilder(_booksInCatalog);
        var catalogProvider = catalogServiceBuilder.Build();

        var bookPriceProvider = new BookPriceRepository();
        var bookMetadataProvider = StubBookMetadataProvider();
        var bookAdvisorHttpClient = StubBookAdvisorHttpClient();

        return new CatalogController(catalogProvider, bookPriceProvider, bookMetadataProvider, bookAdvisorHttpClient);
    }
    private IProvideBookMetadata StubBookMetadataProvider()
    {

        var bookMetadataProvider = Substitute.For<IProvideBookMetadata>();

        foreach (var book in _booksInCatalog)
        {
            bookMetadataProvider.Get(book.Isbn)
                .Returns(new BookReference(book.Isbn, book.Title, book.Author, book.NumberOfPages, book.PictureUrl));
        }
        return bookMetadataProvider;
    }

    private BookAdvisorHttpClient StubBookAdvisorHttpClient()
    {
        return new BookAdvisorHttpClient(new HttpClient(new MockBookAdvisorHttpHandler(_booksInCatalog))
        {
            BaseAddress = new Uri("http://fake-base-address-for-tests")
        });
    }
}