using System.Text;
using BookShop.domain.Catalog;

namespace BookShop.domain.Checkout;

public class UnavailableBooks : Exception
{
    public UnavailableBooks(IReadOnlyCollection<(ISBN isbn, Book? Book)> books) : base(GenerateMessage(books)) {}

    private static string GenerateMessage(IReadOnlyCollection<(ISBN isbn, Book? Book)> books)
    {
        var sb = new StringBuilder("Books are no longer available : ");

        foreach (var (isbn, book) in books)
        {
            if (book is null)
            {
                sb.AppendLine($" - Book with ISBN {isbn} is not in the catalog anymore.");
            }
            else
            {
                sb.AppendLine($" - Stock for {book.Reference.Title} (isbn: {book.Reference.Id}) is not sufficient. Remaining {book.Quantity} items");
            }
        }

        return sb.ToString();
    }
}