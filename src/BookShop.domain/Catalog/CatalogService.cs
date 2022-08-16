namespace BookShop.domain.Catalog; 

public class CatalogService : IProvideCatalog
{
    private readonly IProvideBookMetadata _bookMetadataProvider;
    private readonly IProvideInventory _inventoryProvider;

    public CatalogService(IProvideBookMetadata bookMetadataProvider, IProvideInventory inventoryProvider)
    {
        _bookMetadataProvider = bookMetadataProvider;
        _inventoryProvider = inventoryProvider;
    }

    public Catalog Get(int pageNumber, int numberOfItemsPerPage)
    {
        var allBookReferences = _bookMetadataProvider.Get();

        var pages = allBookReferences
            .Chunk(numberOfItemsPerPage)
            .ToList();

        var bookReferencesToReturn = pages.ElementAt(pageNumber - 1);
        var booksToReturn = _inventoryProvider.Get(bookReferencesToReturn);

        return new Catalog(booksToReturn.ToList(), pages.Count);
    }

    public BookReference Get(ISBN isbn)
    {
        return _bookMetadataProvider.Get(isbn);
    }
}

public interface IProvideCatalog
{
    Catalog Get(int pageNumber, int numberOfItemsPerPage);
}

public interface IProvideInventory
{
    IEnumerable<Book> Get(IEnumerable<BookReference> bookReferences);
}