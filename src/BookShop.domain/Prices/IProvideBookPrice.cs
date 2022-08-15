namespace BookShop.domain.Prices;

public interface IProvideBookPrice
{
    Price GetPrice(ISBN bookId, string currency);
}