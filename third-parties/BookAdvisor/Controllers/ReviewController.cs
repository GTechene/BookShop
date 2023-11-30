using BookShop.shared;
using Microsoft.AspNetCore.Mvc;

namespace BookAdvisor.Controllers;

[ApiController]
[Route("reviews")]
public class ReviewController : ControllerBase
{
    [HttpGet]
    [Route("ratings/{isbn}")]
    public RatingsResponse GetRatings([FromRoute] string isbn)
    {
        return new RatingsResponse(4.3m, 4);
    }
}