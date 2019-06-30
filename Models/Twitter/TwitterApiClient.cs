using EgosaToolAPI.Models.Twitter.Response;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace EgosaToolAPI.Models.Twitter
{
    public class TwitterApiClient
    {
        private static HttpClient Client = new HttpClient();
        private readonly IConfiguration config = null;

        public TwitterApiClient(IConfiguration config)
        {
            this.config = config;
        }

        /// <summary>
        /// Twitterのつぶやきを取得する
        /// Twitter WebAPI仕様: https://developer.twitter.com/en/docs/tweets/search/api-reference/get-search-tweets
        /// </summary>
        public async Task<TwitterApiResponse> get(String sinceId, String maxId, String query)
        {
            var url = $"https://api.twitter.com/1.1/search/tweets.json?q={HttpUtility.UrlEncode(query)}&lang=ja&result_type=mixed&count=100&since_id={HttpUtility.UrlEncode(sinceId)}"; 
            if (!String.IsNullOrEmpty(maxId))
            {
                url += $"&max_id={HttpUtility.UrlEncode(maxId)}";
            }
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var tokenKey = "twitter-token";
            request.Headers.Add("authorization", $"Bearer {config[tokenKey]}");

            var response = await Client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TwitterApiResponse>(json);
        }
    }
}
