using BookShop.domain.Catalog;
using BookShop.domain.Pricing.Prices;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CatalogController : ControllerBase
{
    private readonly IProvideCatalog catalogProvider;
    private readonly IProvideBookPrice bookPriceProvider;

    public CatalogController(IProvideCatalog catalogProvider, IProvideBookPrice bookPriceProvider)
    {
        this.catalogProvider = catalogProvider;
        this.bookPriceProvider = bookPriceProvider;
    }
    
    [HttpGet]
    public CatalogResponse GetCatalog(string currency)
    {
        var catalog = catalogProvider.Get();

        return new CatalogResponse(
            catalog.Books.Select(book => new BookResponse(
                book.Reference.Id.ToString(),
                book.Reference.Title,
                book.Reference.Author,
                book.Reference.PictureUrl.ToString(),
                book.Quantity.Amount,
                bookPriceProvider.GetPrice(book.Reference.Id, currency)
            )).ToArray()
        );
    }

    public record CatalogResponse(BookResponse[] Books);

    public record BookResponse(string ISBN, string Title, string Author, string PictureUrl, int Quantity, Price UnitPrice);
}
