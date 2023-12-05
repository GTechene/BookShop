using BookShop.shared;

namespace BookAdvisor.Repositories;

public interface IReviewRepository
{
    RatingsResponse? GetReviews(string isbn);
}