using BookShop.domain.Catalog;
using BookShop.domain.Prices;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CatalogController : ControllerBase {
    private readonly IProvideBookPrice _bookPriceProvider;
    private readonly IProvideCatalog _catalogService;

    public CatalogController(IProvideCatalog catalogProvider, IProvideBookPrice bookPriceProvider)
    {
        _catalogService = catalogProvider;
        _bookPriceProvider = bookPriceProvider;
    }

    [HttpGet]
    public CatalogResponse GetCatalog(string currency, int pageNumber = 1, int numberOfItemsPerPage = 5)
    {
        var catalog = _catalogService.Get(pageNumber, numberOfItemsPerPage);
        var booksToSend = catalog.Books
            .Where(book => book is not UnknownBook)
            .Select(book => new BookResponse(
                book.Reference.Id.ToString(),
                book.Reference.Title,
                book.Reference.Author,
                book.Reference.PictureUrl!.ToString(),
                book.Quantity.Amount,
                _bookPriceProvider.GetPrice(book.Reference.Id, currency)
            ));

        return new CatalogResponse(
            booksToSend.ToArray(),
            catalog.NumberOfPages
        );
    }

    public record CatalogResponse(BookResponse[] Books, int TotalNumberOfPages);

    public record BookResponse(string ISBN, string Title, string Author, string PictureUrl, int Quantity, Price UnitPrice);
}