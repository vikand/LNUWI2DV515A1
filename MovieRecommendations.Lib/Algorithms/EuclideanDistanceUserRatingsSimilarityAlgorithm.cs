using System;
using System.Collections.Generic;
using System.Linq;
using MovieRecommendations.Entities;
using MovieRecommendations.Lib.Helpers;

namespace MovieRecommendations.Lib.Algorithms
{
    public class EuclideanDistanceUserRatingsSimilarityAlgorithm : IUserRatingsSimilarityAlgorithm
    {
        public double CalculateSimilarity(IEnumerable<Rating> userARatings, IEnumerable<Rating> userBRatings)
        {
            var userAMatchingRatings = userARatings.Where(r1 => userBRatings.Any(r2 => r2.MovieId == r1.MovieId));
            if (!userAMatchingRatings.Any())
                return 0;

            var sumOfSquares = userAMatchingRatings
                .Select(r1 => Math.Pow(r1.UserRating - userBRatings.First(r2 => r2.MovieId == r1.MovieId).UserRating, 2))
                .Sum();

            //return MathHelper.Inverse(1 + Math.Sqrt(sumOfSquares));
            return MathHelper.Inverse(1 + sumOfSquares);
        }
    }
}

