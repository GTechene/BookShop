using BookShop.domain.Catalog;
using BookShop.domain.Pricing.Prices;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CatalogController : ControllerBase
{
    private readonly IProvideCatalog _catalogProvider;
    private readonly IProvideBookPrice _bookPriceProvider;

    public CatalogController(IProvideCatalog catalogProvider, IProvideBookPrice bookPriceProvider)
    {
        _catalogProvider = catalogProvider;
        _bookPriceProvider = bookPriceProvider;
    }
    
    [HttpGet]
    public CatalogResponse GetCatalog(string currency)
    {
        var catalog = _catalogProvider.Get();

        return new CatalogResponse(
            catalog.Books.Select(book => new BookResponse(
                book.Reference.Id.ToString(),
                book.Reference.Title,
                book.Reference.Author,
                book.Reference.PictureUrl.ToString(),
                book.Quantity.Amount,
                _bookPriceProvider.GetPrice(book.Reference.Id, currency)
            )).ToArray()
        );
    }

    public record CatalogResponse(BookResponse[] Books);

    public record BookResponse(string ISBN, string Title, string Author, string PictureUrl, int Quantity, Price UnitPrice);
}
