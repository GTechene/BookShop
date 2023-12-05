using BookShop.domain.Prices;

namespace BookShop.domain.Receipt;

public record ReceiptDetails(BookDetails[] BookDetails, Price PaidPrice);

public record UnknownReceipt() : ReceiptDetails(Array.Empty<BookDetails>(), Price.Null());