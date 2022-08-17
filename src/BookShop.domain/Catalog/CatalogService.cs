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
        var books = _inventoryProvider.Get(bookReferencesToReturn);

        //TODO : handle UnknownBooks here. If any, it means there's a discrepancy between the metadata referential and the inventory referential.
        var knownBooks = books.Where(book => book is not UnknownBook).ToList();
        
        return new Catalog(knownBooks, pages.Count);
    }
}