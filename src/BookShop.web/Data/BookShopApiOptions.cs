namespace BookShop.web.Data;

// ReSharper disable once ClassNeverInstantiated.Global
public class BackendApiOptions {
    public Uri Uri { get; set; } = null!;
}

public record BookShopApiOptions {
    public const string SectionName = "BookShopApi";

    public BackendApiOptions Catalog { get; set; } = null!;
    public BackendApiOptions Price { get; set; } = null!;
}