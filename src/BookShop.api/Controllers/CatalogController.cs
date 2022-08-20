using BookShop.domain;
using BookShop.domain.Catalog;
using BookShop.domain.Prices;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CatalogController : ControllerBase {
    private readonly IProvideBookPrice _bookPriceProvider;
    private readonly IProvideCatalog _catalogService;
    private readonly IProvideBookMetadata _bookMetadata;

    public CatalogController(IProvideCatalog catalogProvider, IProvideBookPrice bookPriceProvider, IProvideBookMetadata bookMetadata)
    {
        _catalogService = catalogProvider;
        _bookPriceProvider = bookPriceProvider;
        _bookMetadata = bookMetadata;
    }

    [HttpGet]
    public CatalogResponse GetCatalog(string currency, int pageNumber = 1, int numberOfItemsPerPage = 5)
    {
        var catalog = _catalogService.Get(pageNumber, numberOfItemsPerPage);
        var books = catalog.Books
            .Select(book => new BookResponse(
                book.Reference.Id.ToString(),
                book.Reference.Title,
                book.Reference.Author,
                book.Reference.PictureUrl!.ToString(),
                book.Quantity.Amount,
                _bookPriceProvider.GetPrice(book.Reference.Id, currency)
            ))
            .ToArray();

        return new CatalogResponse(
            books,
            catalog.NumberOfPages
        );
    }

    public record CatalogResponse(BookResponse[] Books, int TotalNumberOfPages);

    public record BookResponse(string ISBN, string Title, string Author, string PictureUrl, int Quantity, Price UnitPrice);

    [HttpGet]
    [Route("{isbn}")]
    public BookReferenceResponse GetBookReference(string isbn)
    {
        var reference = _bookMetadata.Get(ISBN.Parse(isbn));

        return new BookReferenceResponse(
            reference.Id.ToString(),
            reference.Title,
            reference.PictureUrl?.ToString());
    }

    public record BookReferenceResponse(string ISBN, string Title, string? PictureUrl);
}