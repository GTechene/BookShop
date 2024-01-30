using Diverse;

namespace BookShop.AcceptanceTests;

public class CatalogListScenario {
    public int NumberOfBooksPerPage { get; private set; } = 5;

    private readonly Fuzzer _fuzzer;

    public BookSpecification[] Books { get; private set; }

    public CatalogListScenario(int? seed = null)
    {
        _fuzzer = new Fuzzer(seed);

        var numberOfBooksToGenerate = _fuzzer.GenerateInteger(1, NumberOfBooksPerPage);
        Books = GenerateRandomBooks(numberOfBooksToGenerate);
    }

    public CatalogListScenario WithNumberOfBooksPerPage(int numberOfBooksPerPage)
    {
        NumberOfBooksPerPage = numberOfBooksPerPage;
        return this;
    }

    public CatalogListScenario WithRandomBooks(int numberOfBooksToGenerate)
    {
        Books = GenerateRandomBooks(numberOfBooksToGenerate);
        return this;
    }

    private BookSpecification[] GenerateRandomBooks(int numberOfBooksToGenerate)
    {
        return Enumerable.Range(1, numberOfBooksToGenerate)
            .Select(_ => new BookSpecification(_fuzzer))
            .ToArray();
    }
}