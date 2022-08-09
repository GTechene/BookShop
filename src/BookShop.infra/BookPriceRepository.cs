using BookShop.domain;
using BookShop.domain.Pricing.Prices;

namespace BookShop.infra;

public class BookPriceRepository : IProvideBookPrice
{
    public Price GetPrice(ISBN bookId, string currency)
    {
        return new Price(8, currency);
    }
}