namespace BookShop.domain.Pricing.Prices;

public interface IProvideBookPrice
{
    Price GetPrice(ISBN bookId);
}