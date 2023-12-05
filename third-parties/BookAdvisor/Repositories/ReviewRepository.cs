using BookShop.shared;

namespace BookAdvisor.Repositories;

public class ReviewRepository : IReviewRepository {
    private readonly Dictionary<string, RatingsResponse> _reviewsByBook = new()
    {
        {"978-133888319-0", new RatingsResponse(4.24m, 62921)},
        {"978-054534919-2", new RatingsResponse(4.45m, 36256)},
        {"978-133888321-3", new RatingsResponse(4.53m, 32003)},
        {"978-133888322-0", new RatingsResponse(4.43m, 29068)},
        {"978-060637017-2", new RatingsResponse(4.57m, 27500)}
    };

    public RatingsResponse? GetReviews(string isbn)
    {
        return _reviewsByBook.TryGetValue(isbn, out var reviews) ? reviews : null;
    }
}