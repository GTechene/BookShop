using BookShop.domain.Catalog;
using BookShop.domain.Checkout;
using BookShop.domain.Checkout.Payment;
using BookShop.domain.Prices;
using BookShop.domain.Pricing;
using BookShop.domain.Pricing.Discounts;
using BookShop.infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<CartPricer>();

builder.Services.AddScoped<IProvideBookPrice, BookPriceRepository>(); 
builder.Services.AddScoped<IProvideDiscountDefinitions, DiscountDefinitionRepository>();


builder.Services.AddSingleton<CatalogRepository>();

builder.Services.AddTransient<IProvideCatalog>(services => services.GetRequiredService<CatalogRepository>());

builder.Services.AddTransient<ILockCatalog>(services => services.GetRequiredService<CatalogRepository>());
builder.Services.AddTransient<IUpdateCatalog>(services => services.GetRequiredService<CatalogRepository>());

builder.Services.AddSingleton<ILogTransaction, TransactionLog>();

builder.Services.AddScoped<CheckoutService>();
builder.Services.AddScoped<IProcessPayment, PaymentProcessor>();


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

