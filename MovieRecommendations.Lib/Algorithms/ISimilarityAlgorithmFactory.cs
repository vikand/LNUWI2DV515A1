using MovieRecommendations.Entities;

namespace MovieRecommendations.Lib.Algorithms
{
    public interface ISimilarityAlgorithmFactory
    {
        ISimilarityAlgorithm Create(SimilarityAlgorithm userSimilarityAlgorithm);
    }
}