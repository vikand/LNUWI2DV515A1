using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MovieRecommendations.Entities;
using MovieRecommendations.WebApi.Repositories;

namespace MovieRecommendationsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingsController(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        // GET api/recommendations
        [HttpGet]
        public ActionResult<IEnumerable<Rating>> Get()
        {
            return _ratingRepository.GetRatings().ToList();
        }
    }
}
