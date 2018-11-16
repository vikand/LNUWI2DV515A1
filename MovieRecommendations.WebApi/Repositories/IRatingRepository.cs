using System.Collections.Generic;
using MovieRecommendations.Entities;

namespace MovieRecommendations.WebApi.Repositories
{
    public interface IRatingRepository
    {
        IEnumerable<Movie> GetMovies();
        IEnumerable<Rating> GetRatings();
    }
}