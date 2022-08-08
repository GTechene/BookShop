using BookShop.domain.Pricing.Discounts;
using BookShop.domain.Pricing.Prices;

namespace BookShop.domain.Pricing;

public class CartPricer
{
    private readonly IProvideDiscountDefinitions discountDefinitionsProvider;
    private readonly IProvideBookPrice bookPriceProvider;

    public CartPricer(IProvideDiscountDefinitions discountDefinitionsProvider, IProvideBookPrice bookPriceProvider)
    {
        this.discountDefinitionsProvider = discountDefinitionsProvider;
        this.bookPriceProvider = bookPriceProvider;
    }
    
    public Price ComputePrice(Cart cart)
    {
        return FindBestPrice(cart);
    }

    private Price ComputeCartPriceWithoutDiscount(Cart cart)
    {
        return cart.Aggregate(Price.Zero, (total, book) => 
            total + bookPriceProvider.GetPrice(book)
        );
    }

    private Price FindBestPrice(Cart cart)
    {
        if (cart.IsEmpty)
        {
            return Price.Zero;
        }

        var applicableDiscountDefinitions = GetApplicableDiscountsDefinitions(cart);

        var price = ComputeCartPriceWithoutDiscount(cart);

        foreach (var applicableDiscountDefinition in applicableDiscountDefinitions)
        {
            var (subCart, remainingCart) = applicableDiscountDefinition.ApplyTo(cart);

            var subCartPrice = ComputeCartPriceWithoutDiscount(subCart);

            var discountedPrice = applicableDiscountDefinition.ApplyDiscount(subCartPrice);

            var bestPriceOnRemainingCart = FindBestPrice(remainingCart);

            var cartPrice = discountedPrice + bestPriceOnRemainingCart;
            
            price = new[] { cartPrice, price }.Min();
        }

        return price!;
    }

    private IEnumerable<DiscountDefinition> GetApplicableDiscountsDefinitions(Cart cart)
    {
        return discountDefinitionsProvider.Get().Where(discount => discount.IsApplicable(cart));
    }
}