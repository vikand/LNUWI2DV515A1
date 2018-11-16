using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieRecommendations.Entities;

namespace MovieRecommendations.WebApp.Pages
{
    public class UsersModel : PageModel
    {
        private readonly IHttpClientWrapper client;

        public UsersModel(IHttpClientWrapper client)
        {
            this.client = client;
        }

        public IEnumerable<User> Users { get; set; } 

        public void OnGet()
        {
            Users = client.Get<IEnumerable<User>>("api/users").Item1;
        }
    }
}