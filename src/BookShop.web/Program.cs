using BookShop.web.Data;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<LogErrorHttpMessageHandler>();

builder.Services.AddHttpClient<CatalogHttpClient>(
    (serviceProvider, httpClient) => {
        var options = serviceProvider.GetRequiredService<IOptions<BookShopApiOptions>>();
        httpClient.BaseAddress = options.Value.Catalog.Uri;
    })
    .AddHttpMessageHandler<LogErrorHttpMessageHandler>();

builder.Services.AddHttpClient<PriceHttpClient>(
    (serviceProvider, httpClient) => {
        var options = serviceProvider.GetRequiredService<IOptions<BookShopApiOptions>>();
        httpClient.BaseAddress = options.Value.Price.Uri;
    })
    .AddHttpMessageHandler<LogErrorHttpMessageHandler>();

builder.Services.AddHttpClient<CheckoutHttpClient>(
    (serviceProvider, httpClient) => {
        var options = serviceProvider.GetRequiredService<IOptions<BookShopApiOptions>>();
        httpClient.BaseAddress = options.Value.Checkout.Uri;
    })
    .AddHttpMessageHandler<LogErrorHttpMessageHandler>();

builder.Services.AddHttpClient<PaymentHttpClient>(
    (serviceProvider, httpClient) => {
        var options = serviceProvider.GetRequiredService<IOptions<BookPalApiOptions>>();
        httpClient.BaseAddress = options.Value.Uri;
    })
    .AddHttpMessageHandler<LogErrorHttpMessageHandler>();

builder.Services.AddHttpClient<ReceiptHttpClient>(
        (serviceProvider, httpClient) => {
            var options = serviceProvider.GetRequiredService<IOptions<BookShopApiOptions>>();
            httpClient.BaseAddress = options.Value.Receipt.Uri;
        })
    .AddHttpMessageHandler<LogErrorHttpMessageHandler>();

builder.Services.AddOptions<BookShopApiOptions>()
    .Bind(builder.Configuration.GetSection(BookShopApiOptions.SectionName))
    .ValidateDataAnnotations();

builder.Services.AddOptions<BookPalApiOptions>()
    .Bind(builder.Configuration.GetSection(BookPalApiOptions.SectionName))
    .ValidateDataAnnotations();

builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");



app.Run();