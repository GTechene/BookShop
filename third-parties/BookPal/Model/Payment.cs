namespace BookPal.Model;

public record Payment(
    Price Price,
    Card Card,
    string Hash
);