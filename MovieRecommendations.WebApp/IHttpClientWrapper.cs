using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieRecommendations.WebApp
{
    public interface IHttpClientWrapper
    {
        Tuple<T, HttpStatusCode> Get<T>(string requestUri) where T : class;
        Task<HttpResponseMessage> GetAsync(string requestUri);
    }
}