using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShop.domain.Catalog;

namespace BookShop.infra;

public class InventoryRepository : IProvideInventory, IUpdateInventory, ILockCatalog
{
    private readonly SemaphoreSlim _lock = new(1);

    public IEnumerable<Book> Get(IEnumerable<BookReference> bookReferences)
    {
        //TODO : get data from DB
        throw new NotImplementedException();
    }

    public void Remove(IReadOnlyCollection<(BookReference Book, Quantity Quantity)> books)
    {
        foreach (var (book, quantity) in books)
        {
            RemoveBook(book, quantity);
        }
    }

    private void RemoveBook(BookReference book, Quantity quantity)
    {
        var existingBook = Get(new List<BookReference>{book})
            .Single(b => b.Reference.Id == book.Id);
        var newBook = existingBook with
        {
            Quantity = existingBook.Quantity - quantity
        };
        //TODO : insert new data into DB

        //var index = _books.IndexOf(existingBook);
        //_books.Insert(index, newBook);
        //_books.Remove(existingBook);
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