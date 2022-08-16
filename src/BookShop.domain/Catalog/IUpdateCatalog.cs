namespace BookShop.domain.Catalog;

public interface IUpdateCatalog {
    void Remove(IReadOnlyCollection<(BookReference Book, Quantity Quantity)> books);
}