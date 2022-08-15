namespace BookShop.domain.Pricing.Discounts;

public interface IProvideDiscountDefinitions {
    IEnumerable<DiscountDefinition> Get();
}