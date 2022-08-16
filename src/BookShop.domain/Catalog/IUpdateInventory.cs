namespace BookShop.domain.Catalog;

// TODO : the naming kinda suck here
public interface IUpdateInventory
{
    void Remove(IReadOnlyCollection<(BookReference Book, Quantity Quantity)> books);
}