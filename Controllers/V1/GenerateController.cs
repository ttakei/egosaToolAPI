using System;
using System.Collections.Generic;
using System.Linq;
using EgosaToolAPI.Models.Db;
using EgosaToolAPI.Models.Chatwork;
using EgosaToolAPI.Models.Twitter;
using EgosaToolAPI.Models.Twitter.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ActionResult<string>> Twitter()
        {
            // 過去に取得したtweetIdの最大値を取得
            var latestComments = await db.Comments
                .Where(a => a.Source == "twitter")
                .OrderByDescending(a => a.SourceCommentId)
                .Take(1)
                .Select(a => a.SourceCommentId)
                .ToListAsync();
            var sinceTwitterCommentId = latestComments.Any() ? latestComments[0] : "0";

            // twitterAPIからtweet取得
            var twitterComments = new List<TwitterApiResponseStatus>();
            var maxTwitterCommentId = "";
            var count = 0;
            while (true)
            {
                var response = await twitter.get(sinceTwitterCommentId, maxTwitterCommentId, "オトギフロンティア");
                twitterComments.AddRange(response.statuses);
                if (response.statuses.Count() < 100 || ++count > 50)
                {
                    break;
                }
                maxTwitterCommentId = Convert.ToString(
                    long.Parse(response.statuses.Min(status => status.idStr)) - 1);
            }

            // ChatWorkに投稿
            foreach (TwitterApiResponseStatus tweet in twitterComments)
            {
                // TODO: roomIdのリストをDBから取得
                var roomIdList = new string[] { "155649037" };

                foreach (var roomId in roomIdList)
                {
                    await chatwork.post(roomId, tweet);
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
                    SourceCommentId = tweet.idStr,
                    PostAt = DateTime.ParseExact(
                        tweet.createdAt,
                        "ddd MMM dd HH:mm:ss K yyyy",
                        System.Globalization.DateTimeFormatInfo.InvariantInfo),
                    Body = tweet.text,
                    SearchedAt = DateTime.Now,
                    SearchWord = "オトギフロンティア"
                };

                db.Comments.Add(comment);
            }
            await db.SaveChangesAsync();

            return "OK";
        }
    }
}
