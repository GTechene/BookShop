namespace BookShop.domain.Catalog;

public interface IUpdateCatalog {
    void Remove(IReadOnlyCollection<(Book Book, Quantity Quantity)> books);
}