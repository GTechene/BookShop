using System.Net.Mime;
using System.Text.Json.Serialization;
using BookPal;
using BookPal.Model;
using BookPal.Services;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddRazorPages();

builder.Services.AddScoped<HashGenerator>();
builder.Services.AddScoped<CardValidationService>();

builder.Services.AddOptions<HashOptions>()
    .Bind(builder.Configuration.GetSection(HashOptions.SectionName))
    .ValidateDataAnnotations();

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


app.UseExceptionHandler(exceptionHandlerApp => {
    exceptionHandlerApp.Run(async context => {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        switch (exception)
        {
            case InvalidCard invalidCard:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(ApiError.FromException(invalidCard));
                break;
            case ValidationUsing3DS2Rejected invalidCredentials:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(ApiError.FromException(invalidCredentials));
                break;
            case {} ex:
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsJsonAsync(ApiError.FromException(ex));
                break;
        }

    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
