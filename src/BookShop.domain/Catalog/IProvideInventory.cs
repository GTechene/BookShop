namespace BookShop.domain.Catalog;

public interface IProvideInventory
{
    IEnumerable<Book> Get(IEnumerable<BookReference> bookReferences);
}