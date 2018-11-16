using System.IO;
using System.Collections.Generic;
using System.Runtime.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MovieRecommendations.Lib.Helpers;

namespace MovieRecommendations.WebApi.Repositories
{
    public class RepositoryBase
    {
        public RepositoryBase(
            IConfiguration configuration, 
            IHostingEnvironment hostingEnvironment,
            ICacheHelper cacheHelper,
            ICsvHelper csvHelper)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            CacheHelper = cacheHelper;
            CsvHelper = csvHelper;
        }

        protected IConfiguration Configuration { get; private set; }
        protected IHostingEnvironment HostingEnvironment { get; private set; }
        protected ICsvHelper CsvHelper { get; private set; }
        protected ICacheHelper CacheHelper { get; private set; }

        protected IEnumerable<string[]> GetCsvData(string relativePathToFileConfigurationKey, string columns)
        {
            if (CacheHelper.Contains(relativePathToFileConfigurationKey))
            {
                return CacheHelper.Get<IEnumerable<string[]>>(relativePathToFileConfigurationKey);
            }

            var rootPath = HostingEnvironment.ContentRootPath;
            var relativePathToUsersFile = Configuration[relativePathToFileConfigurationKey];
            var csvFileFieldSeparator = Configuration["CsvFileFieldSeparator"];
            var path = Path.Combine(rootPath, relativePathToUsersFile);

            var csvData = CsvHelper.ReadAllLines(path, columns, csvFileFieldSeparator[0]);

            var cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { path }));
            CacheHelper.Set(relativePathToFileConfigurationKey, csvData, cacheItemPolicy);

            return csvData;
        }
    }
}
