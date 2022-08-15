namespace BookShop.domain.Catalog;

public record BookReference(
    ISBN Id,
    string Title,
    string Author,
    Uri PictureUrl
);