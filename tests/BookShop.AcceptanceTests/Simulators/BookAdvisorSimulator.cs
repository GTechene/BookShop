using BookShop.infra;
using BookShop.shared;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Net;

namespace BookShop.AcceptanceTests.Simulators;

public class BookAdvisorSimulator
{
    private const string FakeBaseAddress = "https://fake-address-for-tests";

    public static void Register(IServiceCollection services)
    {
        services.AddTransient(_ => new BookAdvisorHttpClient(new HttpClient(new StubHttpMessageHandler())
        {
            BaseAddress = new Uri(FakeBaseAddress)
        }));
    }
}

public class StubHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new RatingsResponse(5m, 1))
        });
    }
}