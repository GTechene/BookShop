using BookShop.domain;
using BookShop.domain.Catalog;
using BookShop.domain.Pricing;
using BookShop.domain.Pricing.Discounts;
using BookShop.domain.Pricing.Prices;
using BookShop.infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<CartPricer>();
builder.Services.AddScoped<IProvideBookPrice, BookPriceRepository>(); 
builder.Services.AddScoped<IProvideDiscountDefinitions, DiscountDefinitionRepository>(); 
builder.Services.AddScoped<IProvideCatalog, CatalogRepository>(); 


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
