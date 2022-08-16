namespace BookShop.domain.Catalog;

public record Book(BookReference Reference, Quantity Quantity);

public record UnknownBook(ISBN Id) : Book(new UnknownBookReference(Id), new UnknownQuantity());