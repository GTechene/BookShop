using BookShop.domain.Catalog;

namespace BookShop.domain.Checkout;

public interface IUpdateCatalog
{
    void Remove(IReadOnlyCollection<(Book Book, Quantity Quantity)> books);
}