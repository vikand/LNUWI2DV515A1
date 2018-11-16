using System;
using System.Collections.Generic;
using System.Linq;
using MovieRecommendations.Entities;

namespace MovieRecommendations.Lib.Algorithms
{
    public class PearsonCorrelationUserRatingsSimilarityAlgorithm : IUserRatingsSimilarityAlgorithm
    {
        public double CalculateSimilarity(IEnumerable<Rating> userARatings, IEnumerable<Rating> userBRatings)
        {
            var userAMatchingRatings = userARatings.Where(r1 => userBRatings.Any(r2 => r2.MovieId == r1.MovieId));
            if (!userAMatchingRatings.Any())
                return 0;

            double sumOfAllScoresForUserA = 0,
                   sumOfAllScoresForUserB = 0,
                   sumOfSquaresOfScoresForUserA = 0,
                   sumOfSquaresOfScoresForUserB = 0,
                   sumProductOfScoresForUserAandB = 0;

            foreach (var userAMatchingRating in userAMatchingRatings)
            {
                var userBMatchingRating = userBRatings.First(r => r.MovieId == userAMatchingRating.MovieId);

                sumOfAllScoresForUserA += userAMatchingRating.UserRating;
                sumOfAllScoresForUserB += userBMatchingRating.UserRating;
                sumOfSquaresOfScoresForUserA += userAMatchingRating.UserRating * userAMatchingRating.UserRating;
                sumOfSquaresOfScoresForUserB += userBMatchingRating.UserRating * userBMatchingRating.UserRating;
                sumProductOfScoresForUserAandB += userAMatchingRating.UserRating * userBMatchingRating.UserRating;
            }

            int numberOfMoviesBothUsersHaveRated = userAMatchingRatings.Count();
            var numerator = sumProductOfScoresForUserAandB -
                (sumOfAllScoresForUserA * sumOfAllScoresForUserB / numberOfMoviesBothUsersHaveRated);
            var denominator = Math.Sqrt(
                (sumOfSquaresOfScoresForUserA - sumOfAllScoresForUserA * sumOfAllScoresForUserA / numberOfMoviesBothUsersHaveRated) *
                (sumOfSquaresOfScoresForUserB - sumOfAllScoresForUserB * sumOfAllScoresForUserB / numberOfMoviesBothUsersHaveRated));

            return denominator == 0 ? 0 : numerator / denominator;
        }
    }
}
