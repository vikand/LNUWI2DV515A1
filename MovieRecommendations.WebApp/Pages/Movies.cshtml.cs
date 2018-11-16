using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieRecommendations.Entities;

namespace MovieRecommendations.WebApp.Pages
{
    public class MoviesModel : PageModel
    {
        private readonly IHttpClientWrapper client;

        public MoviesModel(IHttpClientWrapper client)
        {
            this.client = client;
        }

        public IEnumerable<Movie> Movies { get; set; } 

        public void OnGet()
        {
            Movies = client.Get<IEnumerable<Movie>>("api/movies").Item1;
        }
    }
}