using BookAdvisor.Repositories;
using BookShop.shared;
using Microsoft.AspNetCore.Mvc;

namespace BookAdvisor.Controllers;

[ApiController]
[Route("reviews")]
public class ReviewController : ControllerBase
{
    private readonly IReviewRepository _reviewRepository;
    public ReviewController(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    [HttpGet]
    [Route("ratings/{isbn}")]
    public ActionResult<RatingsResponse> GetRatings([FromRoute] string isbn)
    {
        var ratingsResponse = _reviewRepository.GetReviews(isbn);
        if (ratingsResponse == null)
        {
            return NotFound();
        }
        return Ok(ratingsResponse);
    }
}