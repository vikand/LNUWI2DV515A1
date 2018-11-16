using System.Collections.Generic;
using System.Linq;
using MovieRecommendations.Lib.Algorithms;
using MovieRecommendations.Entities;

namespace MovieRecommendations.Lib
{
    public class MovieRecommendationsFinder : IMovieRecommendationsFinder
    {
        private readonly IUserRatingsSimilarityAlgorithmFactory userRatingsSimilarityAlgorithmFactory;

        public MovieRecommendationsFinder(IUserRatingsSimilarityAlgorithmFactory userRatingsSimilarityAlgorithmFactory)
        {
            this.userRatingsSimilarityAlgorithmFactory = userRatingsSimilarityAlgorithmFactory;
        }

        public IEnumerable<Recommendation> FindRecommendations(
            User userToFindRecommendationsFor, 
            IEnumerable<User> allUsers, 
            IEnumerable<Movie> allMovies, 
            IEnumerable<Rating> allRatings,
            UserSimilarityAlgorithm userSimilarityAlgorithm)
        {
            //allUsers = allUsers.Where(u => u.Id != 3).ToList();
            //allRatings = allRatings.Where(r => r.UserId != 3).ToList();

            IUserRatingsSimilarityAlgorithm userRatingsSimilarityAlgorithm =
                this.userRatingsSimilarityAlgorithmFactory.Create(userSimilarityAlgorithm);

            var userToFindRecommendationsForRatings = 
                allRatings.Where(r => r.UserId == userToFindRecommendationsFor.Id);
            var allOtherUsers =
                allUsers.Where(u => u.Id != userToFindRecommendationsFor.Id);
            var allOtherUsersRatings =
                allRatings.Where(r => r.UserId != userToFindRecommendationsFor.Id);
            var unseenMovies = 
                allMovies.Where(m => !userToFindRecommendationsForRatings.Any(r => r.MovieId == m.Id));

            var userSimilarities = allOtherUsers
                .Select(u => new
                {
                    UserId = u.Id,
                    Similarity = userRatingsSimilarityAlgorithm.CalculateSimilarity(
                        userToFindRecommendationsForRatings,
                        allOtherUsersRatings.Where(r => r.UserId == u.Id))
                });

            var movieSimilaritiesAndWeightedRatings = unseenMovies
                .Join(allOtherUsersRatings, m => m.Id, r => r.MovieId, (m, r) => new { m, r })
                .Join(userSimilarities, mr => mr.r.UserId, us => us.UserId, (mr, us) => new { mr, us })
                .Select(mrus => new
                {
                    mrus.mr.r.MovieId,
                    //mrus.mr.r.UserId,
                    //mrus.mr.r.UserRating,
                    mrus.us.Similarity,
                    WeightedRating = mrus.mr.r.UserRating * mrus.us.Similarity,
                });

            var movieScores = movieSimilaritiesAndWeightedRatings
                .GroupBy(msawr => msawr.MovieId)
                .Select(g => new
                {
                    MovieId = g.Key,
                    WeightedRatingSum = g.Sum(msawr => msawr.WeightedRating),
                    SimilaritySum = g.Sum(msawr => msawr.Similarity)
                });

            return movieScores
               .Join(allMovies, ms => ms.MovieId, m => m.Id, (ms, m) => new Recommendation
               {
                   MovieId = m.Id,
                   MovieName = m.Name,
                   Score = ms.WeightedRatingSum / ms.SimilaritySum
               });
        }
    }
}
