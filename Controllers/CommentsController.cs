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
        private readonly ApplicationDbContext _db = null;

        public CommentsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET api/comments
        [HttpGet]
        public ActionResult<string> Get(String tag_group, String source)
        {
            // 全コメントの取得
            return JsonConvert.SerializeObject(
                _db.Comments.OrderByDescending(a => a.Id));
        }
    }
}
