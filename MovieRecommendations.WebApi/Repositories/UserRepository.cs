using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MovieRecommendations.Entities;
using MovieRecommendations.Lib.Helpers;

namespace MovieRecommendations.WebApi.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        private List<User> _users;

        public UserRepository(
            IConfiguration configuration, 
            IHostingEnvironment hostingEnvironment,
            ICacheHelper cacheHelper,
            ICsvHelper csvHelper) : 
            base(configuration, hostingEnvironment, cacheHelper, csvHelper)
        {
        }

        public IEnumerable<User> GetUsers()
        {
            if (_users == null)
            {
                _users = GetAllUsers();
            }

            return _users;
        }

        private List<User> GetAllUsers()
        {
            var users = new List<User>();
            var userData = this.GetCsvData("RelativePathToUsersFile", "UserName;UserID");
            var ratingsData = this.GetCsvData("RelativePathToRatingsFile", "UserID;Movie;Rating");

            foreach (var line in userData)
            {
                if (int.TryParse(line[1], out var id))
                {
                    users.Add(new User
                    {
                        Id = id,
                        Name = line[0],
                        NumberOfSeenMovies = ratingsData.Count(r => r[0] == line[1])
                    });
                }
                else
                {
                    throw new InvalidDataException("Unable to parse user file");
                }
            }

            return users;
        }
    }
}
