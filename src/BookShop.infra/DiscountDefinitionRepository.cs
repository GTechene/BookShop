using BookShop.domain.Pricing.Discounts;
using BookShop.domain.Pricing.Discounts.Targets;
using BookShop.domain.Pricing.Discounts.Types;

namespace BookShop.infra;

public class DiscountDefinitionRepository : IProvideDiscountDefinitions {
    public IEnumerable<DiscountDefinition> Get()
    {
        yield return new DiscountDefinition(NoDiscountType.Instance, AllBooksTarget.Instance);
        yield return new DiscountDefinition(new PercentageDiscountType(5), new DistinctBooksTitleTarget(2));
        yield return new DiscountDefinition(new PercentageDiscountType(10), new DistinctBooksTitleTarget(3));
        yield return new DiscountDefinition(new PercentageDiscountType(20), new DistinctBooksTitleTarget(4));
        yield return new DiscountDefinition(new PercentageDiscountType(25), new DistinctBooksTitleTarget(5));
    }
}