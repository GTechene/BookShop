namespace BookShop.web.Data;

// ReSharper disable once ClassNeverInstantiated.Global
public record Book(string ISBN, string Title, string Author, string PictureUrl, int Quantity, Price UnitPrice);