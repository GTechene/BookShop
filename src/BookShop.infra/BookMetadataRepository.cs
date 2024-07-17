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
            "https://embed.cdn.pais.scholastic.com/v1/products/identifiers/isbn/9780545349253/primary/renditions/700"));

    private static readonly BookReference TheDarkSecret = new(
        ISBN.Parse("978-133888322-0"),
        "The Dark Secret (Wings of Fire #4)",
        "Tui T. Sutherland",
        354,
        new Uri(
            "https://embed.cdn.pais.scholastic.com/v1/products/identifiers/isbn/9780545349260/primary/renditions/700"));

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
            "https://cdn11.bigcommerce.com/s-gibnfyxosi/images/stencil/1280w/products/61935/63763/51UtW35MgDL__48483.1615522638.jpg?c=1"));

    private static readonly BookReference WinterTurning = new(
        ISBN.Parse("978-0545685375"),
        "Winter Turning (Wings of Fire #7)",
        "Tui T. Sutherland",
        336,
        new Uri(
            "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1419320915i/23604435.jpg"));

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
    public BookMetadataForReceipt GetMetadataForReceipt(ISBN isbn)
    {
        var reference = _books.SingleOrDefault(book => book.Id == isbn);

        if (reference == null)
        {
            return new UnknownBookMetadataForReceipt(isbn);
        }

        return new BookMetadataForReceipt(reference.Title, reference.Author, reference.PictureUrl);
    }

    private static IEnumerable<BookReference> GetBooks()
    {
        yield return TheDragonetProphecy;
        yield return TheLostHeir;
        yield return TheHiddenKingdom;
        yield return TheDarkSecret;
        yield return TheBrightestNight;
        yield return MoonRising;
        yield return WinterTurning;
    }
}