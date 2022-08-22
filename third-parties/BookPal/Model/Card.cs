namespace BookPal.Model;

public record Card(
    string Number,
    string OwnerName,
    DateTime ExpirationDate,
    string SecurityCode);