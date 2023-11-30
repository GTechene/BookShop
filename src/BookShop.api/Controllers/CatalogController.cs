using BookShop.domain;
using BookShop.domain.Catalog;
using BookShop.domain.Prices;
using BookShop.infra;
using BookShop.shared;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CatalogController : ControllerBase {
    private readonly IProvideBookPrice _bookPriceProvider;
    private readonly IProvideCatalog _catalogService;
    private readonly IProvideBookMetadata _bookMetadata;
    private readonly BookAdvisorHttpClient _bookAdvisorHttpClient;

    public CatalogController(IProvideCatalog catalogProvider, IProvideBookPrice bookPriceProvider, IProvideBookMetadata bookMetadata, BookAdvisorHttpClient bookAdvisorHttpClient)
    {
        _catalogService = catalogProvider;
        _bookPriceProvider = bookPriceProvider;
        _bookMetadata = bookMetadata;
        _bookAdvisorHttpClient = bookAdvisorHttpClient;
    }

    [HttpGet]
    public async Task<CatalogResponse> GetCatalog(string currency, int pageNumber = 1, int numberOfItemsPerPage = 5)
    {
        var catalog = _catalogService.Get(pageNumber, numberOfItemsPerPage);
        var booksTasks = catalog.Books
            .Select(async book => {
                var unitPrice = _bookPriceProvider.GetPrice(book.Reference.Id, currency);
                var ratings = await _bookAdvisorHttpClient.GetRatings(book.Reference.Id);
                
                return new BookResponse(
                    book.Reference.Id.ToString(),
                    book.Reference.Title,
                    book.Reference.Author,
                    book.Reference.NumberOfPages,
                    ratings,
                    book.Reference.PictureUrl!.ToString(),
                    book.Quantity.Amount,
                    new shared.Price(unitPrice.Amount, unitPrice.Currency)
                );
            });
        var books = await Task.WhenAll(booksTasks);

        return new CatalogResponse(
            books,
            catalog.NumberOfPages
        );
    }

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
}