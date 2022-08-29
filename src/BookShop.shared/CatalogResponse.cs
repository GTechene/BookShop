﻿namespace BookShop.shared; 

public record CatalogResponse(BookResponse[] Books, int TotalNumberOfPages);

public record BookResponse(string ISBN, string Title, string Author, string PictureUrl, int Quantity, Price UnitPrice);