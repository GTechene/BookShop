namespace BookShop.domain.Catalog;

// TODO : the naming kinda suck here
public interface IUpdateInventory
{
    void RemoveCopiesOfBooks(IReadOnlyCollection<(BookReference Book, Quantity Quantity)> books);
}