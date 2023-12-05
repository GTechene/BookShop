namespace BookShop.domain.Receipt;

public record Transaction(string Id, ISBN[] Books, decimal Price, string Currency);