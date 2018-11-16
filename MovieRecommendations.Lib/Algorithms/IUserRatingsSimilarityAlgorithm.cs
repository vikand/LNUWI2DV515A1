using MovieRecommendations.Entities;
using System.Collections.Generic;

namespace MovieRecommendations.Lib.Algorithms
{
    public interface IUserRatingsSimilarityAlgorithm
    {
        double CalculateSimilarity(IEnumerable<Rating> userARatings, IEnumerable<Rating> userBRatings);
    }
}
