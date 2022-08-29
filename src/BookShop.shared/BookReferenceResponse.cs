namespace BookShop.shared; 

public record BookReferenceResponse(string ISBN, string Title, string? PictureUrl);