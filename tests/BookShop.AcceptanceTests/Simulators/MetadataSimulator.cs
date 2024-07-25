using BookShop.domain.Catalog;
using NSubstitute;
using sas.Scenario;
using sas.simulators.nsubstitute;

namespace BookShop.AcceptanceTests.Simulators;

public class MetadataSimulator : BaseSimulator<IProvideBookMetadata>
{
    protected override void Simulate(BaseScenario baseScenario)
    {
        if (baseScenario is not CatalogListScenario scenario)
        {
            return;
        }

        var bookReferences = scenario.Books.Select(book => book.ToBookReference()).ToList();
        Instance.Get().Returns(bookReferences);
    }
}