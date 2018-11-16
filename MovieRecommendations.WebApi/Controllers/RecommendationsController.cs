using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MovieRecommendations.Entities;
using MovieRecommendations.Lib;
using MovieRecommendations.WebApi.Repositories;

namespace MovieRecommendationsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IRatingRepository ratingRepository;
        private readonly IMovieRecommendationsFinder movieRecommendationsFinder;

        public RecommendationsController(
            IUserRepository userRepository,
            IRatingRepository ratingRepository,
            IMovieRecommendationsFinder movieRecommendationsFinder)
        {
            this.userRepository = userRepository;
            this.ratingRepository = ratingRepository;
            this.movieRecommendationsFinder = movieRecommendationsFinder;
        }

        // GET api/users
        //[HttpGet]
        [HttpGet("{userId}/{algorithm}", Name = "Get")]
        public ActionResult<IEnumerable<Recommendation>> Get(int userId, string algorithm)
        {
            var userSimilarityAlgorithm = Enum.Parse<UserSimilarityAlgorithm>(algorithm, true);

            var users = this.userRepository.GetUsers();
            var movies = this.ratingRepository.GetMovies();
            var ratings = this.ratingRepository.GetRatings();

            var recommendations = movieRecommendationsFinder.FindRecommendations(
                users.First(u => u.Id == userId),
                users,
                movies,
                ratings,
                userSimilarityAlgorithm);

            return recommendations.OrderByDescending(r => r.Score).ToList();
        }
    }
}
