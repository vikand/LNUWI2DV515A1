using MovieRecommendations.Entities;
using System;

namespace MovieRecommendations.Lib.Algorithms
{
    public class SimilarityAlgorithmFactory : ISimilarityAlgorithmFactory
    {
        public ISimilarityAlgorithm Create(SimilarityAlgorithm userSimilarityAlgorithm)
        {
            switch (userSimilarityAlgorithm)
            {
                case SimilarityAlgorithm.Euclidean:
                    return new EuclideanDistanceSimilarityAlgorithm();

                case SimilarityAlgorithm.Pearson:
                    return new PearsonCorrelationSimilarityAlgorithm();

                default:
                    throw new ArgumentException(
                        $"Unexpected value: '{userSimilarityAlgorithm}'", 
                        nameof(userSimilarityAlgorithm));
            }
        }
    }
}
