using System.Collections.Generic;
using BookShop.domain.Pricing;
using BookShop.domain.Pricing.Discounts;
using BookShop.domain.Pricing.Discounts.Targets;
using BookShop.domain.Pricing.Discounts.Types;
using BookShop.domain.Pricing.Prices;
using NFluent;
using NSubstitute;
using Xunit;

namespace BookShop.domain.Tests;

public class CartPricerShould
{
    private readonly List<DiscountDefinition> availableDiscountsDefinitions = new()
    {
        new DiscountDefinition(NoDiscountType.Instance, AllBooksTarget.Instance),
        new DiscountDefinition(new PercentageDiscountType(5), new DistinctBooksTitleTarget(2)),
        new DiscountDefinition(new PercentageDiscountType(10), new DistinctBooksTitleTarget(3)),
        new DiscountDefinition(new PercentageDiscountType(20), new DistinctBooksTitleTarget(4)),
        new DiscountDefinition(new PercentageDiscountType(25), new DistinctBooksTitleTarget(5)),
    };
    private static class Books
    {
        public static readonly ISBN TheDragonetProphecy = ISBN.Parse("978-1338883190");
        public static readonly ISBN TheLostHeir = ISBN.Parse("978-0545349192");
        public static readonly ISBN TheHiddenKingdom = ISBN.Parse("978-1338883213");
        public static readonly ISBN TheDarkSecret = ISBN.Parse("978-1338883220");
        public static readonly ISBN TheBrightestNight = ISBN.Parse("978-0606370172");
    }

    private const decimal BookPrice = 8m;

    private const decimal NoDiscountValue = 1;
    private const decimal TwoBooksDiscountValue = 2 * (1 - 0.05m);
    private const decimal ThreeBooksDiscountValue = 3 * (1 - 0.10m);
    private const decimal FourBooksDiscountValue = 4 * (1 - 0.20m);
    private const decimal FiveBooksDiscountValue = 5 * (1 - 0.25m);

    private readonly CartPricer pricer;

    public CartPricerShould()
    {
        var discountDefinitionProvider = Substitute.For<IProvideDiscountDefinitions>();
        discountDefinitionProvider.Get().Returns(availableDiscountsDefinitions);

        var bookPriceProvider = Substitute.For<IProvideBookPrice>();
        bookPriceProvider.GetPrice(Arg.Any<ISBN>()).Returns(BookPrice);
        
        pricer = new CartPricer(discountDefinitionProvider, bookPriceProvider);
    }

    [Fact]
    public void Apply_the_same_price_when_containing_an_order_six_books_with_the_same_title()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheLostHeir, Books.TheLostHeir, 
                Books.TheLostHeir, Books.TheLostHeir, Books.TheLostHeir);

        var price = pricer.ComputePrice(cart);

        Check.That(price).IsEqualTo((Price)(BookPrice * 6 * NoDiscountValue));
    }

    [Fact]
    public void Apply_a_five_percent_discount_when_containing_an_order_with_two_books_with_two_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret);

        var price = pricer.ComputePrice(cart);

        Check.That(price).IsEqualTo((Price)(BookPrice * TwoBooksDiscountValue));
    }

    [Fact]
    public void Apply_the_five_percent_discount_only_on_the_two_books_with_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret)
            .Add(Books.TheLostHeir, Books.TheDarkSecret)
            .Add(Books.TheDarkSecret);

        var price = pricer.ComputePrice(cart);
        Check.That(price).IsEqualTo((Price)(BookPrice * (TwoBooksDiscountValue + TwoBooksDiscountValue + NoDiscountValue)));
    }

    [Fact]
    public void Apply_a_ten_percent_discount_when_containing_an_order_with_three_books_with_three_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret, Books.TheDragonetProphecy);

        var price = pricer.ComputePrice(cart);

        Check.That(price).IsEqualTo((Price)(BookPrice * ThreeBooksDiscountValue));
    }

    [Fact]
    public void Apply_the_ten_percent_discount_only_on_the_three_books_with_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret, Books.TheDragonetProphecy)
            .Add(Books.TheLostHeir);

        var price = pricer.ComputePrice(cart);

        Check.That(price).IsEqualTo((Price)(BookPrice * (ThreeBooksDiscountValue + NoDiscountValue)));
    }

    [Fact]
    public void Apply_a_twenty_percent_discount_when_containing_an_order_with_four_books_with_four_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret, Books.TheDragonetProphecy, Books.TheHiddenKingdom);

        var price = pricer.ComputePrice(cart);
            
        Check.That(price).IsEqualTo((Price)(BookPrice * FourBooksDiscountValue));
    }

    [Fact]
    public void Apply_the_twenty_percent_discount_only_on_the_four_books_with_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret, Books.TheDragonetProphecy, Books.TheHiddenKingdom)
            .Add(Books.TheHiddenKingdom);

        var price = pricer.ComputePrice(cart);

        Check.That(price).IsEqualTo((Price)(BookPrice * (FourBooksDiscountValue + NoDiscountValue)));
    }

    [Fact]
    public void Apply_a_twenty_five_percent_discount_when_containing_an_order_with_five_books_with_five_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret, Books.TheDragonetProphecy, Books.TheHiddenKingdom, Books.TheBrightestNight);

        var price = pricer.ComputePrice(cart);

        Check.That(price).IsEqualTo((Price)(BookPrice * FiveBooksDiscountValue));
    }

    [Fact]
    public void Apply_a_twenty_five_percent_discount_only_to_the_five_books_with_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret, Books.TheDragonetProphecy, Books.TheHiddenKingdom, Books.TheBrightestNight)
            .Add(Books.TheHiddenKingdom)
            .Add(Books.TheHiddenKingdom);

        var price = pricer.ComputePrice(cart);

        var expectedDiscountValue = (FiveBooksDiscountValue + NoDiscountValue + NoDiscountValue);

        Check.That(price).IsEqualTo((Price)(BookPrice * expectedDiscountValue));
    }

    [Fact]
    public void Apply_the_best_discount_in_favor_to_the_customer()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheLostHeir)
            .Add(Books.TheDarkSecret, Books.TheDarkSecret)
            .Add(Books.TheDragonetProphecy, Books.TheDragonetProphecy)
            .Add(Books.TheHiddenKingdom)
            .Add(Books.TheBrightestNight);

        var price = pricer.ComputePrice(cart);

        Check.That(price).IsEqualTo((Price)(BookPrice * (FourBooksDiscountValue + FourBooksDiscountValue)));
    }
}