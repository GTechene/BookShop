using BookShop.domain.Catalog;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace BookShop.AcceptanceTests.Simulators;

public class MetadataSimulator
{
    private readonly IProvideBookMetadata _metadataProvider;

    public MetadataSimulator(CatalogListScenario scenario)
    {
        _metadataProvider = Substitute.For<IProvideBookMetadata>();
        Simulate(scenario);
    }

    private void Simulate(CatalogListScenario scenario)
    {
        var bookReferences = scenario.Books.Select(book => book.ToBookReference()).ToList();
        _metadataProvider.Get().Returns(bookReferences);
    }

    public void Register(IServiceCollection services)
    {
        services.AddTransient(_ => _metadataProvider);
    }
}