using System.Collections;
using System.Collections.Immutable;
using BookShop.domain.Pricing.Discounts;
using BookShop.domain.Pricing.Discounts.Types;
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
    
    public (Price, AppliedDiscounts) ComputePrice(Cart cart, string currency)
    {
        return FindBestPrice(cart, currency);
    }

    private Price ComputeCartPriceWithoutDiscount(Cart cart, string currency)
    {
        return cart.Aggregate(Price.Zero(currency), (total, book) => 
            total + bookPriceProvider.GetPrice(book, currency)
        );
    }

    private (Price, AppliedDiscounts) FindBestPrice(Cart cart, string currency)
    {
        if (cart.IsEmpty)
        {
            return (Price.Zero(currency), AppliedDiscounts.Empty);
        }

        var applicableDiscountDefinitions = GetApplicableDiscountsDefinitions(cart);

        var price = ComputeCartPriceWithoutDiscount(cart, currency);

        var discounts = AppliedDiscounts.Empty;

        foreach (var applicableDiscountDefinition in applicableDiscountDefinitions)
        {
            var (appliedDiscount, remainingCart) = applicableDiscountDefinition.ApplyTo(cart);

            var subCartPrice = ComputeCartPriceWithoutDiscount(appliedDiscount.Cart, currency);

            var discountedPrice = appliedDiscount.Apply(subCartPrice);

            var (bestPriceOnRemainingCart, appliedDiscounts) = FindBestPrice(remainingCart, currency);

            var cartPrice = discountedPrice + bestPriceOnRemainingCart;

            if (cartPrice < price)
            {
                price = cartPrice;
                discounts = AppliedDiscounts.Empty
                    .Add(appliedDiscount)
                    .Append(appliedDiscounts);

            }
        }

        return (price, discounts);
    }

    private IEnumerable<DiscountDefinition> GetApplicableDiscountsDefinitions(Cart cart)
    {
        return discountDefinitionsProvider.Get().Where(discount => discount.IsApplicable(cart));
    }
}

public record AppliedDiscount(Cart Cart, DiscountType Type)
{
    public Price Apply(Price price)
    {
        return Type.Apply(price);
    }
}   

public class AppliedDiscounts : IEnumerable<AppliedDiscount>
{
    private ImmutableList<AppliedDiscount> AppliedDiscountsList { get; init; } = ImmutableList<AppliedDiscount>.Empty;

    public static readonly AppliedDiscounts Empty = new();

    public AppliedDiscounts Add(AppliedDiscount appliedDiscount)
    {
        return new AppliedDiscounts
        {
            AppliedDiscountsList = AppliedDiscountsList.Add(appliedDiscount)
        };
    }
    
    public AppliedDiscounts Append(AppliedDiscounts appliedDiscounts)
    {
        return new AppliedDiscounts
        {
            AppliedDiscountsList = appliedDiscounts.Aggregate(AppliedDiscountsList, (discounts, discount) => discounts.Add(discount))
        };
    }
    
    public IEnumerator<AppliedDiscount> GetEnumerator()
    {
        return AppliedDiscountsList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
