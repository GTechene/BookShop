using System.Collections.Generic;
using BookShop.domain.Prices;
using BookShop.domain.Pricing;
using BookShop.domain.Pricing.Discounts;
using BookShop.domain.Pricing.Discounts.Targets;
using BookShop.domain.Pricing.Discounts.Types;
using NFluent;
using NSubstitute;
using Xunit;

namespace BookShop.domain.Tests;

public class CartPricerShould
{
    private readonly List<DiscountDefinition> _availableDiscountsDefinitions = new()
    {
        new DiscountDefinition(NoDiscountType.Instance, AllBooksTarget.Instance),
        new DiscountDefinition(new PercentageDiscountType(5), new DistinctBooksTitleTarget(2)),
        new DiscountDefinition(new PercentageDiscountType(10), new DistinctBooksTitleTarget(3)),
        new DiscountDefinition(new PercentageDiscountType(20), new DistinctBooksTitleTarget(4)),
        new DiscountDefinition(new PercentageDiscountType(25), new DistinctBooksTitleTarget(5)),
    };
    private static class Books
    {
        public static readonly ISBN TheDragonetProphecy = ISBN.Parse("978-133888319-0");
        public static readonly ISBN TheLostHeir = ISBN.Parse("978-054534919-2");
        public static readonly ISBN TheHiddenKingdom = ISBN.Parse("978-133888321-3");
        public static readonly ISBN TheDarkSecret = ISBN.Parse("978-133888322-0");
        public static readonly ISBN TheBrightestNight = ISBN.Parse("978-060637017-2");
    }

    private const decimal BookPrice = 8m;

    private const decimal NoDiscountValue = 1;
    private const decimal TwoBooksDiscountValue = 2 * (1 - 0.05m);
    private const decimal ThreeBooksDiscountValue = 3 * (1 - 0.10m);
    private const decimal FourBooksDiscountValue = 4 * (1 - 0.20m);
    private const decimal FiveBooksDiscountValue = 5 * (1 - 0.25m);
    private const string Currency = "EUR";

    private readonly CartPricer _pricer;

    public CartPricerShould()
    {
        var discountDefinitionProvider = Substitute.For<IProvideDiscountDefinitions>();
        discountDefinitionProvider.Get().Returns(_availableDiscountsDefinitions);

        var bookPriceProvider = Substitute.For<IProvideBookPrice>();
        bookPriceProvider.GetPrice(Arg.Any<ISBN>(), Arg.Any<string>()).Returns(new Price(BookPrice, Currency));
        
        _pricer = new CartPricer(discountDefinitionProvider, bookPriceProvider);
    }

    [Fact]
    public void Apply_the_same_price_when_containing_an_order_six_books_with_the_same_title()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheLostHeir, Books.TheLostHeir, 
                Books.TheLostHeir, Books.TheLostHeir, Books.TheLostHeir);

        var (price, _) = _pricer.ComputePrice(cart, Currency);

        Check.That(price).IsEqualTo(new Price(BookPrice * 6 * NoDiscountValue, Currency));
    }

    [Fact]
    public void Apply_a_five_percent_discount_when_containing_an_order_with_two_books_with_two_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret);

        var (price, _) = _pricer.ComputePrice(cart, Currency);

        Check.That(price).IsEqualTo(new Price(BookPrice * TwoBooksDiscountValue, Currency));
    }

    [Fact]
    public void Apply_the_five_percent_discount_only_on_the_two_books_with_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret)
            .Add(Books.TheLostHeir, Books.TheDarkSecret)
            .Add(Books.TheDarkSecret);

        var (price, _) = _pricer.ComputePrice(cart, Currency);
        Check.That(price).IsEqualTo(new Price(BookPrice * (TwoBooksDiscountValue + TwoBooksDiscountValue + NoDiscountValue), Currency));
    }

    [Fact]
    public void Apply_a_ten_percent_discount_when_containing_an_order_with_three_books_with_three_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret, Books.TheDragonetProphecy);

        var (price, _) = _pricer.ComputePrice(cart, Currency);

        Check.That(price).IsEqualTo(new Price(BookPrice * ThreeBooksDiscountValue, Currency));
    }

    [Fact]
    public void Apply_the_ten_percent_discount_only_on_the_three_books_with_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret, Books.TheDragonetProphecy)
            .Add(Books.TheLostHeir);

        var (price, _) = _pricer.ComputePrice(cart, Currency);

        Check.That(price).IsEqualTo(new Price(BookPrice * (ThreeBooksDiscountValue + NoDiscountValue), Currency));
    }

    [Fact]
    public void Apply_a_twenty_percent_discount_when_containing_an_order_with_four_books_with_four_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret, Books.TheDragonetProphecy, Books.TheHiddenKingdom);

        var (price, _) = _pricer.ComputePrice(cart, Currency);
            
        Check.That(price).IsEqualTo(new Price(BookPrice * FourBooksDiscountValue, Currency));
    }

    [Fact]
    public void Apply_the_twenty_percent_discount_only_on_the_four_books_with_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret, Books.TheDragonetProphecy, Books.TheHiddenKingdom)
            .Add(Books.TheHiddenKingdom);

        var (price, _) = _pricer.ComputePrice(cart, Currency);

        Check.That(price).IsEqualTo(new Price(BookPrice * (FourBooksDiscountValue + NoDiscountValue), Currency));
    }

    [Fact]
    public void Apply_a_twenty_five_percent_discount_when_containing_an_order_with_five_books_with_five_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret, Books.TheDragonetProphecy, Books.TheHiddenKingdom, Books.TheBrightestNight);

        var (price, _) = _pricer.ComputePrice(cart, Currency);

        Check.That(price).IsEqualTo(new Price(BookPrice * FiveBooksDiscountValue, Currency));
    }

    [Fact]
    public void Apply_a_twenty_five_percent_discount_only_to_the_five_books_with_different_titles()
    {
        var cart = Cart.Empty
            .Add(Books.TheLostHeir, Books.TheDarkSecret, Books.TheDragonetProphecy, Books.TheHiddenKingdom, Books.TheBrightestNight)
            .Add(Books.TheHiddenKingdom)
            .Add(Books.TheHiddenKingdom);

        var (price, _) = _pricer.ComputePrice(cart, Currency);

        var expectedDiscountValue = (FiveBooksDiscountValue + NoDiscountValue + NoDiscountValue);

        Check.That(price).IsEqualTo(new Price(BookPrice * expectedDiscountValue, Currency));
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

        var (price, _) = _pricer.ComputePrice(cart, Currency);

        Check.That(price).IsEqualTo(new Price(BookPrice * (FourBooksDiscountValue + FourBooksDiscountValue), Currency));
    }
}