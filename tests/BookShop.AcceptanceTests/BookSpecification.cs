using BookShop.domain;
using BookShop.domain.Catalog;
using Diverse;

namespace BookShop.AcceptanceTests;

public class BookSpecification
{
    public ISBN Isbn { get; }
    public Quantity Quantity { get; }
    public Uri PictureUrl { get; }
    public int NumberOfPages { get; }
    public string Author { get; }
    public string Title { get; }
    public int NumberOfRatings { get; }
    public decimal AverageRating { get; }

    public BookSpecification(IFuzz fuzzer)
    {
        Isbn = fuzzer.GenerateIsbn10();
        Title = fuzzer.GenerateSentence(6);
        var firstName = fuzzer.GenerateFirstName();
        var lastName = fuzzer.GenerateLastName(firstName);
        Author = $"{firstName} {lastName}";
        NumberOfPages = fuzzer.GenerateInteger(10, 1500);
        PictureUrl = new Uri(fuzzer.GenerateStringFromPattern("http://picture-url-for-tests/xxxxxxx.jpg"));
        Quantity = new Quantity(fuzzer.GenerateInteger(1, 99));
        AverageRating = Math.Round(fuzzer.GeneratePositiveDecimal(0m, 5m), 2);
        NumberOfRatings = fuzzer.GenerateInteger(2, 20000);
    }

    public Book ToBook()
    {
        var bookReference = ToBookReference();
        return new Book(bookReference, Quantity);
    }

    public BookReference ToBookReference()
    {
        return new BookReference(Isbn, Title, Author, NumberOfPages, PictureUrl);
    }
}