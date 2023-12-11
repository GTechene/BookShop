using System.Net;
using System.Net.Http.Json;
using BookShop.shared;
using Diverse;

namespace BookShop.AcceptanceTests.Builders;

internal class MockBookAdvisorHttpHandler(IFuzzNumbers fuzzer) : HttpMessageHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri is not null && request.RequestUri.AbsolutePath.StartsWith("/reviews/ratings/"))
        {
            var rating = Math.Round(fuzzer.GeneratePositiveDecimal(0m, 5m), 2);
            var numberOfRatings = fuzzer.GenerateInteger(2, 20000);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new RatingsResponse(rating, numberOfRatings))
            };
        }

        return new HttpResponseMessage(HttpStatusCode.NotFound);
    }
}