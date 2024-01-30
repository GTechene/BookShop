using BookShop.domain.Catalog;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace BookShop.AcceptanceTests.Simulators;

public class InventorySimulator
{
    private readonly IProvideInventory _inventoryProvider;

    public InventorySimulator(CatalogListScenario scenario)
    {
        _inventoryProvider = Substitute.For<IProvideInventory>();
        Simulate(scenario);
    }

    private void Simulate(CatalogListScenario scenario)
    {
        var books = scenario.Books.Select(book => book.ToBook()).ToList();
        _inventoryProvider.Get(Arg.Any<IEnumerable<BookReference>>())
            .Returns(callInfo =>
            {
                var requestedBooksIsbns = callInfo.Arg<IEnumerable<BookReference>>();
                return books.IntersectBy(requestedBooksIsbns, book => book.Reference);
            });
    }

    public void Register(IServiceCollection services)
    {
        services.AddTransient(_ => _inventoryProvider);
    }
}