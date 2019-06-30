using EgosaToolAPI.Models.Twitter.Response;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace EgosaToolAPI.Models.Chatwork
{
    public class ChatworkApiClient
    {
        private static HttpClient Client = new HttpClient();

        private readonly IConfiguration config = null;

        public ChatworkApiClient(IConfiguration config)
        {
            this.config = config;
        }

        /// <summary>
        /// Chatworkに投稿する
        /// Chatwork WebAPI仕様: http://developer.chatwork.com/ja/endpoint_rooms.html#POST-rooms-room_id-messages
        /// </summary>
        public async Task<HttpResponseMessage> post(String roomId, TwitterApiResponseStatus tweet)
        {
            var url = $"https://api.chatwork.com/v2/rooms/{HttpUtility.UrlEncode(roomId)}/messages";
            var postBody = $"[hr][info]{tweet.text}[/info]by {tweet.user.name}  at {tweet.createdAt}";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "body", postBody },
                { "self_unread", "0" }
            });

            var tokenKey = $"chatwork-room-{roomId}-token";
            request.Headers.Add("X-ChatWorkToken", config[tokenKey]);

            var response = await Client.SendAsync(request);
            return response;
        }
    }
}
