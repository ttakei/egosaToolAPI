using System;
using System.Collections.Generic;
using System.Linq;
using EgosaToolAPI.Models.Db;
using EgosaToolAPI.Models.Chatwork;
using EgosaToolAPI.Models.Twitter;
using EgosaToolAPI.Models.Twitter.Response;
using Microsoft.AspNetCore.Mvc;

namespace EgosaToolAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class GenerateController : ControllerBase
    {
        private readonly ApplicationDbContext db = null;
        private readonly TwitterApiClient twitter = null;
        private readonly ChatworkApiClient chatwork = null;

        public GenerateController(
            ApplicationDbContext db,
            TwitterApiClient twitter,
            ChatworkApiClient chatwork
        )
        {
            this.db = db;
            this.twitter = twitter;
            this.chatwork = chatwork;
        }

        public ActionResult<string> Twitter()
        {
            // 過去に取得したtweetIdの最大値を取得
            string sinceTwitterCommentId = db.Comments
                .Where(c => c.Source == "twitter")
                .Select(c => c.SourceCommentId)
                .Max() ?? "0";

            // twitterAPIからtweet取得
            var twitterComments = new List<TwitterApiResponseStatus>();
            var maxTwitterCommentId = "";
            var count = 0;
            while (true)
            {
                var task = twitter.get(sinceTwitterCommentId, maxTwitterCommentId, "オトギフロンティア");
                task.Wait();
                var response = task.Result;
                twitterComments.AddRange(response.statuses);
                if (response.statuses.Count() < 100 || ++count > 50)
                {
                    break;
                }
                maxTwitterCommentId = Convert.ToString(
                    long.Parse(response.statuses.Min(status => status.id_str)) - 1);
            }

            // ChatWorkに投稿
            foreach (TwitterApiResponseStatus tweet in twitterComments)
            {
                // TODO: roomIdのリストをDBから取得
                var roomIdList = new string[] { "155649037" };

                foreach (var roomId in roomIdList)
                {
                    // ChatworkApiへの負荷を考慮し同期実行
                    chatwork.post(roomId, tweet).Wait();
                }
            }


            // DBに格納
            twitterComments.Sort();
            foreach (TwitterApiResponseStatus tweet in twitterComments)
            {
                var comment = new Comment
                {
                    CommentTagSetId = 1,
                    Source = "twitter",
                    SourceCommentId = tweet.id_str,
                    PostAt = DateTime.ParseExact(
                        tweet.created_at,
                        "ddd MMM dd HH:mm:ss K yyyy",
                        System.Globalization.DateTimeFormatInfo.InvariantInfo),
                    Body = tweet.text,
                    SearchedAt = DateTime.Now,
                    SearchWord = "オトギフロンティア"
                };

                db.Comments.Add(comment);
            }
            db.SaveChanges();

            return "OK";
        }
    }
}
