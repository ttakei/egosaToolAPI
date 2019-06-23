using EgosaToolAPI.Models.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EgosaToolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        // GET api/comments
        [HttpGet]
        public ActionResult<string> Get(String tag_group, String source)
        {
            // 全コメントの取得
            using (var context = new CommentsContext())
            {
                return JsonConvert.SerializeObject(
                    context.Comments.OrderByDescending(a => a.Id));
            }
        }
    }
}
