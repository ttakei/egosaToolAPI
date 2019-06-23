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

namespace EgosaToolAPI.Models.Twitter
{
    public class TwitterApiClient : IDisposable
    {
        private static HttpClient Client = new HttpClient();

        // see https://developer.twitter.com/en/docs/tweets/search/api-reference/get-search-tweets
        public async Task<TwitterApiResponse> get(String sinceId, String maxId, String query)
        {
            /*
            Client.DefaultRequestHeaders.Accept.Clear();
            */
            String url = String.Format("https://api.twitter.com/1.1/search/tweets.json?q={0}&lang=ja&result_type=mixed&count=100&since_id={1}", 
                HttpUtility.UrlEncode(query), HttpUtility.UrlEncode(sinceId));
            if (!String.IsNullOrEmpty(maxId))
            {
                url += String.Format("&max_id={0}", HttpUtility.UrlEncode(maxId));
            }
            var request = new HttpRequestMessage(HttpMethod.Get, @url);
            // request.Headers.Add("Content-Type", @"application/json");
            // TODO: BearerIdの隠ぺい
            request.Headers.Add(@"authorization", @"Bearer AAAAAAAAAAAAAAAAAAAAAMC7%2FAAAAAAAUFgxRY%2BjD4mq67NqkMeXzZgGhnc%3DAPM0Lz0lRutO3bmQJmOBXO5so3zXxHMPJ9UXsYLqGe53hBzeIX");
            var response = await Client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TwitterApiResponse>(json);
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
