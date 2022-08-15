using BookShop.domain.Catalog;

namespace BookShop.domain.Checkout;

internal class UnavailableBooks : Exception
{
    public IReadOnlyCollection<(ISBN isbn, Book? Book)> Books { get; }

    public UnavailableBooks(IReadOnlyCollection<(ISBN isbn, Book? Book)> books)
    {
        this.Books = books;
        throw new NotImplementedException();
    }
}