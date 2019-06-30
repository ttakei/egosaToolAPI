using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EgosaToolAPI.Models.Db;
using EgosaToolAPI.Models.Chatwork;
using EgosaToolAPI.Models.Twitter;
using EgosaToolAPI.Models.Twitter.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EgosaToolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateController : ControllerBase
    {
        private readonly ApplicationDbContext _db = null;

        public GenerateController(ApplicationDbContext db)
        {
            _db = db;
        }

        // POST: api/generate/twitter
        [HttpPost]
        public void PostTwitter()
        {
            // 過去に取得したtweetIdの最大値を取得
            String sinceTwitterCommentId;
            sinceTwitterCommentId = _db.Comments
                .Where(c => c.Source == "twitter")
                .Select(c => c.SourceCommentId)
                .Max() ?? "0";

            // twitterAPIからtweet取得
            // TODO: 定数化orパラメタ化
            // TODO: 最新100件しか取れてない
            var twitterComments = new List<TwitterApiResponseStatus>();
            String maxTwitterCommentId = "";
            using (var client = new TwitterApiClient()) {
                int count = 0;
                while (true)
                {
                    var task = client.get(sinceTwitterCommentId, maxTwitterCommentId, "オトギフロンティア");
                    //task.Start();
                    //task.Wait();
                    var response = task.Result;
                    twitterComments.AddRange(response.statuses);
                    if (response.statuses.Count() < 100 || ++count > 50)
                    {
                        break;
                    }
                    maxTwitterCommentId = Convert.ToString(
                        long.Parse(response.statuses.Min(status => status.id_str)) - 1);
                }
            }

            // ChatWorkに投稿
            using (var client = new ChatworkApiClient())
            {
                foreach (TwitterApiResponseStatus tweet in twitterComments)
                {
                    // TODO: roomIdのリストをDBから取得
                    var task = client.post("155649037", tweet);
                }
            }


            // DBに格納
            // TODO: レスポンスの変換処理を別の場所に移動
            twitterComments.Sort();
            foreach (TwitterApiResponseStatus tweet in twitterComments)
            {
                var comment = new Comment
                {
                    // TODO: タグ管理
                    CommentTagSetId = 1,
                    // TODO: 定数化
                    Source = "twitter",
                    SourceCommentId = tweet.id_str,
                    PostAt = DateTime.ParseExact(
                        tweet.created_at,
                        "ddd MMM dd HH:mm:ss K yyyy",
                        System.Globalization.DateTimeFormatInfo.InvariantInfo),
                    Body = tweet.text,
                    SearchedAt = DateTime.Now,
                    // TODO: 定数化orパラメータ化
                    SearchWord = "オトギフロンティア"
                };

                _db.Comments.Add(comment);
            }
            _db.SaveChanges();
        }
    }
}
