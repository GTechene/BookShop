using BookShop.domain.Catalog;
using NSubstitute;
using sas.Scenario;
using sas.simulators.nsubstitute;

namespace BookShop.AcceptanceTests.Simulators;

public class InventorySimulator : BaseSimulator<IProvideInventory>
{
    protected override void Simulate(BaseScenario baseScenario)
    {
        if (baseScenario is not CatalogListScenario scenario)
        {
            return;
        }

        var books = scenario.Books.Select(book => book.ToBook()).ToList();
        Instance.Get(Arg.Any<IEnumerable<BookReference>>())
            .Returns(callInfo =>
            {
                var requestedBooksIsbns = callInfo.Arg<IEnumerable<BookReference>>();
                return books.IntersectBy(requestedBooksIsbns, book => book.Reference);
            });
    }
}