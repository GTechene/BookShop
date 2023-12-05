namespace BookShop.shared;

public record CommandResponse(ReceiptBookResponse[] Books, Price PaidPrice);

public record ReceiptBookResponse(string Title, string Author, string PictureUrl, int Quantity, Price UnitPrice);