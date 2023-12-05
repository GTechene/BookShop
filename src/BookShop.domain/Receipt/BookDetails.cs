using BookShop.domain.Prices;

namespace BookShop.domain.Receipt;

public record BookDetails(string Title, string Author, Uri? PictureUrl, int OrderedQuantity, Price UnitPrice);