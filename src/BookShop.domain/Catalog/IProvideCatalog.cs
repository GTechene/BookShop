namespace BookShop.domain.Catalog;

public interface IProvideCatalog
{
    Catalog Get(int pageNumber, int numberOfItemsPerPage);
}