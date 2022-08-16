using BookShop.domain;
using BookShop.domain.Catalog;

namespace BookShop.infra;

public class CatalogRepository : IProvideCatalog, ILockCatalog, IUpdateCatalog {

    // ReSharper disable once IdentifierTypo
    private static readonly BookReference TheDragonetProphecy = new(
        ISBN.Parse("978-133888319-0"),
        "The Dragonet Prophecy (Wings of Fire #1)",
        "Tui T. Sutherland",
        new Uri(
            "https://s2.qwant.com/thumbr/0x380/7/e/dd4e8783e856ec64127dc9f47f0dc6746c8fa6d56d6ce36e173818f4d4c3e7/latest.jpg?u=https%3A%2F%2Fvignette3.wikia.nocookie.net%2Fwingsoffire%2Fimages%2F6%2F62%2F11.jpg%2Frevision%2Flatest%3Fcb%3D20170614012225&q=0&b=1&p=0&a=0"));

    private static readonly BookReference TheLostHeir = new(
        ISBN.Parse("978-054534919-2"),
        "The Lost Heir (Wings of Fire #2)",
        "Tui T. Sutherland",
        new Uri(
            "https://s2.qwant.com/thumbr/0x380/6/7/99129d301bb33a6fe827579d3978bac1636ed3224b6278def5209446085b14/700.jpg?u=https%3A%2F%2Fembed.cdn.pais.scholastic.com%2Fv1%2Fchannels%2Fsso%2Fproducts%2Fidentifiers%2Fisbn%2F9780545349246%2Fprimary%2Frenditions%2F700%3FuseMissingImage%3Dtrue&q=0&b=1&p=0&a=0"));

    private static readonly BookReference TheHiddenKingdom = new(
        ISBN.Parse("978-133888321-3"),
        "The Hidden Kingdom (Wings of Fire #3)",
        "Tui T. Sutherland",
        new Uri(
            "https://s1.qwant.com/thumbr/0x380/b/0/1f6b01fca18c48b39ab5b28f5e1823039bd249d6f71034ebab4eec95053410/71bjmewcntl-1.jpg?u=https%3A%2F%2Froguewatson.files.wordpress.com%2F2019%2F05%2F71bjmewcntl-1.jpg&q=0&b=1&p=0&a=0"));

    private static readonly BookReference TheDarkSecret = new(
        ISBN.Parse("978-133888322-0"),
        "The Dark Secret (Wings of Fire #4)",
        "Tui T. Sutherland",
        new Uri(
            "https://s2.qwant.com/thumbr/0x380/c/f/7c6941c5f16aece63bd4f11a4d1e936303de4c141bd2d3e748ab20d685ae8d/9780545349260.jpg?u=https%3A%2F%2Fimages.thenile.io%2Fr1000%2F9780545349260.jpg&q=0&b=1&p=0&a=0"));

    private static readonly BookReference TheBrightestNight = new(
        ISBN.Parse("978-060637017-2"),
        "The Brightest Night (Wings of Fire #5)",
        "Tui T. Sutherland",
        new Uri(
            "https://s1.qwant.com/thumbr/0x380/1/e/23b6f89fe9f2666c323ad3231dc97fe8cf72eef5dc8eb2a9f59953f657fde8/9780545349222_0.jpg?u=https%3A%2F%2Fd5i0fhmkm8zzl.cloudfront.net%2F9780545349222_0.jpg&q=0&b=1&p=0&a=0"));

    private static readonly BookReference MoonRising = new(
        ISBN.Parse("978-0545685368"),
        "Moon Rising (Wings of Fire #6)",
        "Tui T. Sutherland",
        new Uri(
            "https://s1.qwant.com/thumbr/0x380/9/9/683b67d0de9c96101174b7fda79181c3b581d8c38dad2ffeb97d9d9cbf496d/9781432874247.jpg?u=https%3A%2F%2Fi.thenile.io%2Fr1000%2F9781432874247.jpg%3Fr%3D5f1ae53b26fd2&q=0&b=1&p=0&a=0"));

    private readonly List<Book> _books = GetBooks().ToList();

    private readonly SemaphoreSlim _lock = new(1);

    public void Lock()
    {
        _lock.Wait();
    }

    public void UnLock()
    {
        _lock.Release();
    }
    public Catalog Get()
    {
        return new Catalog(_books);
    }

    public void Remove(IReadOnlyCollection<(BookReference Book, Quantity Quantity)> books)
    {
        foreach (var (book, quantity) in books)
        {
            RemoveBook(book, quantity);
        }
    }


    private static IEnumerable<Book> GetBooks()
    {
        yield return new Book(TheDragonetProphecy, 30);
        yield return new Book(TheLostHeir, 20);
        yield return new Book(TheHiddenKingdom, 20);
        yield return new Book(TheDarkSecret, 15);
        yield return new Book(TheBrightestNight, 5);
        yield return new Book(MoonRising, 6);
    }

    private void RemoveBook(BookReference book, Quantity quantity)
    {
        var existingBook = _books.Single(b => b.Reference.Id == book.Id);
        var newBook = existingBook with
        {
            Quantity = existingBook.Quantity - quantity
        };
        var index = _books.IndexOf(existingBook);
        _books.Insert(index, newBook);
        _books.Remove(existingBook);
    }
}