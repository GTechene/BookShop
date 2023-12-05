namespace BookShop.domain.Catalog;

public record BookMetadataForReceipt(string Title, string Author, Uri? PictureUrl);
public record UnknownBookMetadataForReceipt(ISBN Id) : BookMetadataForReceipt(string.Empty, string.Empty, null);