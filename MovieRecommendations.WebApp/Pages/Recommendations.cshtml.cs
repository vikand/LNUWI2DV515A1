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

        public IDictionary<UserSimilarityAlgorithm, IEnumerable<Recommendation>> Recommendations { get; private set; }

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
                Recommendations = new Dictionary<UserSimilarityAlgorithm, IEnumerable<Recommendation>>();

                if (UseEuclidean)
                {
                    Recommendations.Add(
                        UserSimilarityAlgorithm.Euclidean, 
                        GetRecommendations(SelectedUser, UserSimilarityAlgorithm.Euclidean));
                }

                if (UsePearson)
                {
                    Recommendations.Add(
                        UserSimilarityAlgorithm.Pearson,
                        GetRecommendations(SelectedUser, UserSimilarityAlgorithm.Pearson));
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

        private IEnumerable<Recommendation> GetRecommendations(int userId, UserSimilarityAlgorithm algorithm)
        {
            return client.Get<IEnumerable<Recommendation>>($"api/recommendations/{userId}/{algorithm}").Item1;
        }
    }
}