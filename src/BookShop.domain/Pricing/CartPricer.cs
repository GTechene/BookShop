﻿using BookShop.domain.Prices;
using BookShop.domain.Pricing.Discounts;

namespace BookShop.domain.Pricing;

// TODO : do we add a port interface here ?
public class CartPricer {
    private readonly IProvideBookPrice _bookPriceProvider;
    private readonly IProvideDiscountDefinitions _discountDefinitionsProvider;

    public CartPricer(IProvideDiscountDefinitions discountDefinitionsProvider, IProvideBookPrice bookPriceProvider)
    {
        _discountDefinitionsProvider = discountDefinitionsProvider;
        _bookPriceProvider = bookPriceProvider;
    }

    public (Price, AppliedDiscounts) ComputePrice(Cart cart, string currency)
    {
        // TODO : do we handle the price conversion here ?
        return FindBestPrice(cart, currency);
    }

    private Price ComputeCartPriceWithoutDiscount(Cart cart, string currency)
    {
        return cart.Aggregate(Price.Zero(currency), func: (total, book) =>
            total + _bookPriceProvider.GetPrice(book, currency)
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
        return _discountDefinitionsProvider.Get().Where(discount => discount.IsApplicable(cart));
    }
}