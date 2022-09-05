namespace BookShop.shared;

public record CommandResponse(BookResponse[] Books, Price PaidPrice);