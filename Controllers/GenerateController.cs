using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EgosaToolAPI.Models.Db;
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
        // POST: api/generate/twitter
        [HttpPost]
        public void PostTwitter()
        {
            // 過去に取得したtweetIdの最大値を取得
            String sinceTwitterCommentId;
            using (var context = new CommentsContext()) {
                sinceTwitterCommentId = context.Comments
                    .Where(c => c.Source == "twitter")
                    .Select(c => c.SourceCommentId)
                    .Max() ?? "0";
            }

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

            // DBに格納
            // TODO: レスポンスの変換処理を別の場所に移動
            using (var context = new CommentsContext())
            {
                twitterComments.Sort();
                foreach (TwitterApiResponseStatus twitterComment in twitterComments)
                {
                    var comment = new Comment
                    {
                        // TODO: タグ管理
                        CommentTagSetId = 1,
                        // TODO: 定数化
                        Source = "twitter",
                        SourceCommentId = twitterComment.id_str,
                        PostAt = DateTime.ParseExact(
                            twitterComment.created_at,
                            "ddd MMM dd HH:mm:ss K yyyy",
                            System.Globalization.DateTimeFormatInfo.InvariantInfo),
                        Body = twitterComment.text,
                        SearchedAt = DateTime.Now,
                        // TODO: 定数化orパラメータ化
                        SearchWord = "オトギフロンティア"
                    };

                    context.Comments.Add(comment);
                }

                context.SaveChanges();
            }
        }
    }
}
