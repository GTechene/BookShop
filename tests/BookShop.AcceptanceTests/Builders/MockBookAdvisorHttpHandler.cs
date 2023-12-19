using System.Net;
using System.Net.Http.Json;
using BookShop.domain;
using BookShop.shared;

namespace BookShop.AcceptanceTests.Builders;

internal class MockBookAdvisorHttpHandler(BookSpecification[] books) : HttpMessageHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri is not null && request.RequestUri.AbsolutePath.StartsWith("/reviews/ratings/"))
        {
            var path = request.RequestUri.AbsolutePath;
            var requestedIsbn = path.Substring(path.LastIndexOf('/') + 1);
            var matchingBook = books.SingleOrDefault(book => book.Isbn == ISBN.Parse(requestedIsbn));
            if (matchingBook != null)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(new RatingsResponse(matchingBook.AverageRating, matchingBook.NumberOfRatings))
                };
            }
        }

        return new HttpResponseMessage(HttpStatusCode.NotFound);
    }
}