namespace BookShop.shared; 

public record CatalogResponse(BookResponse[] Books, int TotalNumberOfPages);

public record BookResponse(string ISBN, string Title, string Author, int NumberOfPages, string PictureUrl, int Quantity, Price UnitPrice);