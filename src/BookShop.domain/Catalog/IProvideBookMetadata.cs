namespace BookShop.domain.Catalog;

public interface IProvideBookMetadata {
    List<BookReference> Get();
    BookReference? Get(ISBN isbn);
}