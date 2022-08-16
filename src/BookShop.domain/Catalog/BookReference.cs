namespace BookShop.domain.Catalog;

public record BookReference(
    ISBN Id,
    string Title,
    string Author,
    Uri? PictureUrl
);

public record UnknownBookReference(ISBN Id) : BookReference(Id, string.Empty, string.Empty, null);