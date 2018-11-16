using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieRecommendations.WebApp
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        public HttpClient httpClient;

        public HttpClientWrapper(string baseAddress)
        {
            httpClient = new HttpClient() { BaseAddress = new Uri(baseAddress) };
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await httpClient.GetAsync(requestUri);
        }

        public Tuple<T,HttpStatusCode> Get<T>(string requestUri) where T : class
        {
            T result = null; 

            var response = GetAsync(requestUri).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<T>().GetAwaiter().GetResult();
            }

            return new Tuple<T, HttpStatusCode>(result, response.StatusCode);
        }
    }
}
