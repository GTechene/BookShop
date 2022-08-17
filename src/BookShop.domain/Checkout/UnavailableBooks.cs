using System.Text;
using BookShop.domain.Catalog;

namespace BookShop.domain.Checkout;

public class UnavailableBooks : Exception
{
    public UnavailableBooks(IReadOnlyCollection<Book> books) : base(GenerateMessage(books)) {}

    private static string GenerateMessage(IReadOnlyCollection<Book> books)
    {
        var sb = new StringBuilder("Books are no longer available : ");

        foreach (var book in books)
        {
            if (book is UnknownBook)
            {
                sb.AppendLine($" - Book with ISBN {book.Reference.Id} is not in the catalog anymore.");
            }
            else
            {
                sb.AppendLine($" - Stock for {book.Reference.Title} (isbn: {book.Reference.Id}) is not sufficient. Remaining {book.Quantity} items");
            }
        }

        return sb.ToString();
    }
}