using BookShop.infra;
using BookShop.shared;
using System.Net.Http.Json;
using System.Net;
using sas.Scenario;
using sas.simulators.http.nsubstitute;
using NSubstitute;

namespace BookShop.AcceptanceTests.Simulators;

public class BookAdvisorSimulator : BaseHttpClientSimulator<BookAdvisorHttpClient>
{
    protected override void Simulate(BaseScenario scenario)
    {
        HttpClient.Get(Arg.Is<string>(route => route.StartsWith("reviews/ratings")))
            .Returns(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new RatingsResponse(5m, 1))
            });
    }
}