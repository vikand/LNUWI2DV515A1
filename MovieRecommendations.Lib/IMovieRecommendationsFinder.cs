using System.Collections.Generic;
using MovieRecommendations.Entities;

namespace MovieRecommendations.Lib
{
    public interface IMovieRecommendationsFinder
    {
        IEnumerable<Recommendation> FindRecommendations(
            User userToFindRecommendationsFor, 
            IEnumerable<User> allUsers, 
            IEnumerable<Movie> allMovies, 
            IEnumerable<Rating> allRatings, 
            UserSimilarityAlgorithm algorithm);
    }
}