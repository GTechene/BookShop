using BookShop.domain;
using BookShop.domain.Pricing.Prices;

namespace BookShop.infra;

public class BookPriceRepository : IProvideBookPrice
{
    public Price GetPrice(ISBN bookId)
    {
        return 8m;
    }
}