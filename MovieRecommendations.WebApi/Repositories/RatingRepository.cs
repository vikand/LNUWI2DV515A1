using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MovieRecommendations.Entities;
using MovieRecommendations.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MovieRecommendations.WebApi.Repositories
{
    public class RatingRepository : RepositoryBase, IRatingRepository
    {
        private class InternalRating
        {
            public int UserId { get; set; }
            public string MovieName { get; set; }
            public float Rating { get; set; }
        }


        private List<Movie> movies;
        private List<Rating> ratings;

        public RatingRepository(
            IConfiguration configuration, 
            IHostingEnvironment hostingEnvironment,
            ICacheHelper cacheHelper,
            ICsvHelper csvHelper) : 
            base(configuration, hostingEnvironment, cacheHelper, csvHelper)
        {
        }

        public IEnumerable<Movie> GetMovies()
        {
            if (this.movies == null)
            {
                GetMoviesAndRatings();
            }

            return this.movies;
        }

        public IEnumerable<Rating> GetRatings()
        {
            if (this.ratings == null)
            {
                GetMoviesAndRatings();
            }

            return this.ratings;
        }

        private void GetMoviesAndRatings()
        {
            var movieId = 0;

            this.movies = new List<Movie>();
            this.ratings = new List<Rating>();

            foreach (var internalRating in GetInternalRatings())
            {
                var movie = this.movies.FirstOrDefault(
                    m => m.Name.Equals(internalRating.MovieName, StringComparison.InvariantCultureIgnoreCase));

                if (movie == null)
                {
                    movie = new Movie { Id = ++movieId, Name = internalRating.MovieName };
                    movies.Add(movie);
                }

                ratings.Add(new Rating
                {
                    MovieId = movie.Id,
                    UserId = internalRating.UserId,
                    UserRating = internalRating.Rating
                });
            }

            this.movies.ForEach(m => m.AverageRating = 
                this.ratings.Where(r => r.MovieId == m.Id).Average(r => r.UserRating));
        }

        private List<InternalRating> GetInternalRatings()
        {
            var internalRatings = new List<InternalRating>();
            var ratingData = GetCsvData("RelativePathToRatingsFile", "UserID;Movie;Rating");
            var fileFormatCulture = CultureInfo.CreateSpecificCulture(Configuration["CsvFileCulture"]);

            foreach (var line in ratingData)
            {
                var internalRating = new InternalRating { MovieName = line[1] };

                if (int.TryParse(line[0], out var userId) &&
                    float.TryParse(line[2], NumberStyles.Float, fileFormatCulture, out var rating))
                {
                    internalRatings.Add(new InternalRating
                    {
                        UserId = userId,
                        MovieName = line[1],
                        Rating = rating
                    });
                }
                else
                {
                    throw new InvalidDataException("Unable to parse rating file");
                }
            }

            return internalRatings;
        }
    }
}
