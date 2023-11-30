namespace BookShop.domain.Catalog;

public record BookReference(
    ISBN Id,
    string Title,
    string Author,
    int NumberOfPages,
    Uri? PictureUrl
);

public record UnknownBookReference(ISBN Id) : BookReference(Id, string.Empty, string.Empty, 0, null);