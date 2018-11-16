using MovieRecommendations.Entities;
using System;

namespace MovieRecommendations.Lib.Algorithms
{
    public class UserRatingsSimilarityAlgorithmFactory : IUserRatingsSimilarityAlgorithmFactory
    {
        public IUserRatingsSimilarityAlgorithm Create(UserSimilarityAlgorithm userSimilarityAlgorithm)
        {
            switch (userSimilarityAlgorithm)
            {
                case UserSimilarityAlgorithm.Euclidean:
                    return new EuclideanDistanceUserRatingsSimilarityAlgorithm();

                case UserSimilarityAlgorithm.Pearson:
                    return new PearsonCorrelationUserRatingsSimilarityAlgorithm();

                default:
                    throw new ArgumentException(
                        $"Unexpected value: '{userSimilarityAlgorithm}'", 
                        nameof(userSimilarityAlgorithm));
            }
        }
    }
}
