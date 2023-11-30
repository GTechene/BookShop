using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.shared;
public record RatingsResponse(decimal AverageRating, int NumberOfRatings);