using EgosaToolAPI.Models.Twitter.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace EgosaToolAPI.Models.Chatwork
{
    public class ChatworkApiClient : IDisposable
    {
        private static HttpClient Client = new HttpClient();

        // see http://developer.chatwork.com/ja/endpoint_rooms.html#POST-rooms-room_id-messages
        public async Task<HttpResponseMessage> post(String roomeId, TwitterApiResponseStatus tweet)
        {
            String url = String.Format("https://api.chatwork.com/v2/rooms/{0}/messages", 
                HttpUtility.UrlEncode(roomeId));
            String message = String.Format(format: "[hr][info]{0}[/info]by {1}  at {2}", tweet.text, tweet.user.name, tweet.created_at);

            var request = new HttpRequestMessage(HttpMethod.Post, @url);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "body", message },
                { "self_unread", "0" }
            });
            request.Headers.Add(@"X-ChatWorkToken", @"5b3b2c71cd2d73b849b67c2c2686c373");
            var response = await Client.SendAsync(request);
            return response;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~TwitterApiClient()
        // {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
