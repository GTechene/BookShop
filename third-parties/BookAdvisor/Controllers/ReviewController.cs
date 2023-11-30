using BookShop.shared;
using Microsoft.AspNetCore.Mvc;

namespace BookAdvisor.Controllers;

[ApiController]
[Route("reviews")]
public class ReviewController : ControllerBase
{
    [HttpGet]
    [Route("ratings")]
    public RatingsResponse GetRatings(string isbn)
    {
        return new RatingsResponse(3.75m, 4);
    }
}