namespace BookShop.domain.Catalog;

public record Catalog(IReadOnlyCollection<Book> Books);