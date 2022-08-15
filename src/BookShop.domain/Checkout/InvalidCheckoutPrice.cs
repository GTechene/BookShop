using BookShop.domain.Prices;

namespace BookShop.domain.Checkout;

public class InvalidCheckoutPrice : Exception {
    public InvalidCheckoutPrice(Price expectedPrice, Price requestedPrice)
        : base(GenerateMessage(expectedPrice, requestedPrice))
    {

    }
    private static string GenerateMessage(Price expectedPrice, Price requestedPrice)
    {
        return $"Requested Price {requestedPrice} does not match the real cart price {expectedPrice}";
    }
}