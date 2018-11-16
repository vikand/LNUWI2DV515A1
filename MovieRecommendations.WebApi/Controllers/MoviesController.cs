using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MovieRecommendations.Entities;
using MovieRecommendations.WebApi.Repositories;

namespace MovieRecommendationsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;

        public MoviesController(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        // GET api/movies
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> Get()
        {
            return _ratingRepository.GetMovies().ToList();
        }
    }
}
