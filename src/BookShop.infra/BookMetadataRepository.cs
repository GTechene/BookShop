using BookShop.domain;
using BookShop.domain.Catalog;

namespace BookShop.infra;

public class BookMetadataRepository : IProvideBookMetadata
{
    private static readonly BookReference TheDragonetProphecy = new(
        ISBN.Parse("978-133888319-0"),
        "The Dragonet Prophecy (Wings of Fire #1)",
        "Tui T. Sutherland",
        336,
        new Uri(
            "https://s2.qwant.com/thumbr/0x0/5/4/3dde4aa99ad8275bf403c085737594fefa3a0c6b011359b3133c455df2570e/.jpg?u=http%3A%2F%2Fwww.scholastic.ca%2Fhipoint%2F648%2F%3Fsrc%3D9780545349239.jpg%26w%3D260&q=0&b=1&p=0&a=0"));

    private static readonly BookReference TheLostHeir = new(
        ISBN.Parse("978-054534919-2"),
        "The Lost Heir (Wings of Fire #2)",
        "Tui T. Sutherland",
        296,
        new Uri(
            "https://s2.qwant.com/thumbr/0x380/6/7/99129d301bb33a6fe827579d3978bac1636ed3224b6278def5209446085b14/700.jpg?u=https%3A%2F%2Fembed.cdn.pais.scholastic.com%2Fv1%2Fchannels%2Fsso%2Fproducts%2Fidentifiers%2Fisbn%2F9780545349246%2Fprimary%2Frenditions%2F700%3FuseMissingImage%3Dtrue&q=0&b=1&p=0&a=0"));

    private static readonly BookReference TheHiddenKingdom = new(
        ISBN.Parse("978-133888321-3"),
        "The Hidden Kingdom (Wings of Fire #3)",
        "Tui T. Sutherland",
        326,
        new Uri(
            "https://s1.qwant.com/thumbr/0x380/b/0/1f6b01fca18c48b39ab5b28f5e1823039bd249d6f71034ebab4eec95053410/71bjmewcntl-1.jpg?u=https%3A%2F%2Froguewatson.files.wordpress.com%2F2019%2F05%2F71bjmewcntl-1.jpg&q=0&b=1&p=0&a=0"));

    private static readonly BookReference TheDarkSecret = new(
        ISBN.Parse("978-133888322-0"),
        "The Dark Secret (Wings of Fire #4)",
        "Tui T. Sutherland",
        354,
        new Uri(
            "https://s2.qwant.com/thumbr/0x380/c/f/7c6941c5f16aece63bd4f11a4d1e936303de4c141bd2d3e748ab20d685ae8d/9780545349260.jpg?u=https%3A%2F%2Fimages.thenile.io%2Fr1000%2F9780545349260.jpg&q=0&b=1&p=0&a=0"));

    private static readonly BookReference TheBrightestNight = new(
        ISBN.Parse("978-060637017-2"),
        "The Brightest Night (Wings of Fire #5)",
        "Tui T. Sutherland",
        335,
        new Uri(
            "https://s1.qwant.com/thumbr/0x380/1/e/23b6f89fe9f2666c323ad3231dc97fe8cf72eef5dc8eb2a9f59953f657fde8/9780545349222_0.jpg?u=https%3A%2F%2Fd5i0fhmkm8zzl.cloudfront.net%2F9780545349222_0.jpg&q=0&b=1&p=0&a=0"));

    private static readonly BookReference MoonRising = new(
        ISBN.Parse("978-0545685368"),
        "Moon Rising (Wings of Fire #6)",
        "Tui T. Sutherland",
        299,
        new Uri(
            "https://s1.qwant.com/thumbr/0x380/9/9/683b67d0de9c96101174b7fda79181c3b581d8c38dad2ffeb97d9d9cbf496d/9781432874247.jpg?u=https%3A%2F%2Fi.thenile.io%2Fr1000%2F9781432874247.jpg%3Fr%3D5f1ae53b26fd2&q=0&b=1&p=0&a=0"));

    // TODO: maybe the book class should be renamed "BookStorageItem" ou "BookItem" and should be kept int the infra layer. Domain only requires a BookReference and a Quantity which can be achieved using a tuple. Book can also be misleading here. Is this really the physical book ? 
    private readonly List<BookReference> _books = GetBooks().ToList();

    
    public List<BookReference> Get()
    {
        return _books;
    }

    public BookReference Get(ISBN isbn)
    {
        var result = _books.SingleOrDefault(book => book.Id == isbn);
        if (result == null)
        {
            return new UnknownBookReference(isbn);
        }

        return result;
    }

    private static IEnumerable<BookReference> GetBooks()
    {
        yield return TheDragonetProphecy;
        yield return TheLostHeir;
        yield return TheHiddenKingdom;
        yield return TheDarkSecret;
        yield return TheBrightestNight;
        yield return MoonRising;
    }
}