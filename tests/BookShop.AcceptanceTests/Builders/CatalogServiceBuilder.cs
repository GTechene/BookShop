using BookShop.domain.Catalog;
using NSubstitute;

namespace BookShop.AcceptanceTests.Builders;

public class CatalogServiceBuilder(BookSpecification[] booksInCatalog)
{
    public CatalogService Build()
    {
        var bookMetadataProvider = StubBookMetadataProvider();
        var inventoryProvider = StubInventoryProvider();

        return new CatalogService(bookMetadataProvider, inventoryProvider);
    }

    private IProvideBookMetadata StubBookMetadataProvider()
    {
        var bookMetadataProvider = Substitute.For<IProvideBookMetadata>();

        bookMetadataProvider.Get()
            .Returns(booksInCatalog.Select(book => new BookReference(book.Isbn, book.Title, book.Author, book.NumberOfPages, book.PictureUrl)).ToList());

        return bookMetadataProvider;
    }

    private IProvideInventory StubInventoryProvider()
    {
        var inventoryProvider = Substitute.For<IProvideInventory>();
        inventoryProvider.Get(Arg.Any<IEnumerable<BookReference>>())
            .Returns(callInfo =>
            {
                var requestedBookReferences = callInfo.Arg<IEnumerable<BookReference>>();

                return booksInCatalog
                    .Where(book => requestedBookReferences.Contains(new BookReference(book.Isbn, book.Title, book.Author, book.NumberOfPages, book.PictureUrl)))
                    .Select(book => book.ToBook());
            });

        return inventoryProvider;
    }
}