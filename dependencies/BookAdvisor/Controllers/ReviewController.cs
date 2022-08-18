using Microsoft.AspNetCore.Mvc;

namespace BookAdvisor.Controllers;

[ApiController]
[Route("reviews")]
public class ReviewController : ControllerBase
{
    [HttpGet]
    [Route("ratings")]
    public IEnumerable<decimal> GetRatings(string isbn)
    {
        return new List<decimal> {3.5m, 5.0m, 4.5m};
    }
}