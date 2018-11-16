using System.Collections.Generic;
using MovieRecommendations.Entities;

namespace MovieRecommendations.WebApi.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
    }
}