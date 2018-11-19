using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MovieRecommendations.Entities;

namespace MovieRecommendations.WebApp.Pages
{
    public class RecommendationsModel : PageModel
    {
        private readonly IMemoryCache cache;
        private readonly IHttpClientWrapper client;

        public RecommendationsModel(IHttpClientWrapper client, IMemoryCache cache)
        {
            this.client = client;
            this.cache = cache;
        }

        public IEnumerable<User> Users { get; private set; }

        public IDictionary<SimilarityAlgorithm, IEnumerable<Recommendation>> Recommendations { get; private set; }

        [BindProperty]
        public int SelectedUser { get; set; }

        [BindProperty]
        public bool UseEuclidean { get; set; }

        [BindProperty]
        public bool UsePearson { get; set; }

        public void OnGet()
        {
            GetUsers(false);
        }

        public void OnPost()
        {
            GetUsers(true);

            if (SelectedUser != 0)
            {
                Recommendations = new Dictionary<SimilarityAlgorithm, IEnumerable<Recommendation>>();

                if (UseEuclidean)
                {
                    Recommendations.Add(
                        SimilarityAlgorithm.Euclidean, 
                        GetRecommendations(SelectedUser, SimilarityAlgorithm.Euclidean));
                }

                if (UsePearson)
                {
                    Recommendations.Add(
                        SimilarityAlgorithm.Pearson,
                        GetRecommendations(SelectedUser, SimilarityAlgorithm.Pearson));
                }
            }
        }

        private void GetUsers(bool useCache)
        {
            IEnumerable<User> users = null;

            if (useCache)
            {
                users = this.cache.Get<IEnumerable<User>>("users");
            }

            if (users == null)
            {
                users = client.Get<IEnumerable<User>>("api/users").Item1;
            }

            Users = users;
        }

        private IEnumerable<Recommendation> GetRecommendations(int userId, SimilarityAlgorithm algorithm)
        {
            return client.Get<IEnumerable<Recommendation>>($"api/recommendations/{userId}/{algorithm}").Item1;
        }
    }
}