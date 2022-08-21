namespace BookShop.web.Data;

public record Card(
    string Number,
    DateTime ExpirationDate,
    string SecurityCode,
    string OwnerName
);