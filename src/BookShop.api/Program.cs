using System.Net;
using BookShop.api.Errors;
using BookShop.domain;
using BookShop.domain.Catalog;
using BookShop.domain.Checkout;
using BookShop.domain.Checkout.Payment;
using BookShop.domain.Prices;
using BookShop.domain.Pricing;
using BookShop.domain.Pricing.Discounts;
using BookShop.infra;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.AddConsole();

builder.Services.AddControllers();

builder.Services.AddScoped<CartPricer>();

builder.Services.AddScoped<IProvideBookPrice, BookPriceRepository>();
builder.Services.AddScoped<IProvideDiscountDefinitions, DiscountDefinitionRepository>();

builder.Services.AddSingleton<BookMetadataRepository>();
builder.Services.AddTransient<IProvideBookMetadata>(services => services.GetRequiredService<BookMetadataRepository>());

builder.Services.AddSingleton<CatalogService>();
builder.Services.AddTransient<IProvideCatalog>(services => services.GetRequiredService<CatalogService>());

builder.Services.AddSingleton<InventoryRepository>();

builder.Services.AddTransient<IProvideInventory>(services => services.GetRequiredService<InventoryRepository>());
builder.Services.AddTransient<ILockCatalog>(services => services.GetRequiredService<InventoryRepository>());
builder.Services.AddTransient<IUpdateInventory>(services => services.GetRequiredService<InventoryRepository>());

builder.Services.AddSingleton<ILogTransaction, TransactionLog>();

builder.Services.AddScoped<CheckoutService>();

builder.Services.AddOptions<BookPalApiOptions>()
    .ValidateDataAnnotations()
    .Bind(builder.Configuration.GetSection(BookPalApiOptions.Section));

builder.Services.AddHttpClient<BookPalApiHttpClient>((serviceProvider, client) => {

    var options = serviceProvider.GetRequiredService<IOptions<BookPalApiOptions>>();
    client.BaseAddress = options.Value.Uri;
});

builder.Services.AddTransient<IProcessPayment>(serviceProvider => serviceProvider.GetRequiredService<BookPalApiHttpClient>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(handler =>
    handler
        .On<ISBN.InvalidIsbn>(HttpStatusCode.BadRequest, LogLevel.Error)
        .On<UnavailableBooks>(HttpStatusCode.BadRequest, LogLevel.Error)
        .On<InvalidCheckoutPrice>(HttpStatusCode.BadRequest, LogLevel.Error)
        .On<PaymentProcessFailed>(HttpStatusCode.BadRequest, LogLevel.Error)
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
