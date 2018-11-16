using MovieRecommendations.Entities;

namespace MovieRecommendations.Lib.Algorithms
{
    public interface IUserRatingsSimilarityAlgorithmFactory
    {
        IUserRatingsSimilarityAlgorithm Create(UserSimilarityAlgorithm userSimilarityAlgorithm);
    }
}