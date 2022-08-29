namespace BookShop.shared;

public record Price(decimal Amount, string Currency);

public record PriceResponse(Price Total, string[] Discounts);