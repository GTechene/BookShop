using BookShop.domain;
using BookShop.domain.Catalog;

namespace BookShop.infra;

public class InventoryRepository : IProvideInventory, IUpdateInventory, ILockCatalog
{
    private readonly SemaphoreSlim _lock = new(1);

    private readonly Dictionary<ISBN, Quantity> _inventoryByISBN = new()
    {
        [ISBN.Parse("978-133888319-0")] = 20,
        [ISBN.Parse("978-054534919-2")] = 12,
        [ISBN.Parse("978-133888321-3")] = 12,
        [ISBN.Parse("978-133888322-0")] = 12,
        [ISBN.Parse("978-060637017-2")] = 5,
        [ISBN.Parse("978-0545685368")] = 12
    };

    public IEnumerable<Book> Get(IEnumerable<BookReference> bookReferences)
    {
        return bookReferences.Select(Get);
    }

    public Book Get(BookReference bookReference)
    {
        if (_inventoryByISBN.ContainsKey(bookReference.Id))
        {
            return new Book(bookReference, _inventoryByISBN[bookReference.Id]);
        }

        return new UnknownBook(bookReference.Id);
    }

    public void RemoveCopiesOfBooks(IReadOnlyCollection<(BookReference Book, Quantity Quantity)> books)
    {
        foreach (var (book, quantity) in books)
        {
            RemoveCopy(book, quantity);
        }
    }

    private void RemoveCopy(BookReference book, Quantity quantity)
    {
        if (_inventoryByISBN.ContainsKey(book.Id))
        {
            _inventoryByISBN[book.Id] -= quantity;
        }
        else
        {
            throw new UnknownBookInInventoryException(book.Id);
        }
    }

    public void Lock()
    {
        _lock.Wait();
    }

    public void UnLock()
    {
        _lock.Release();
    }
}