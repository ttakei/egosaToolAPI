using EgosaToolAPI.Models.Db;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace EgosaToolAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext db = null;

        public CommentsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ActionResult<string> Get()
        {
            // 全コメントの取得
            return JsonConvert.SerializeObject(
                db.Comments.OrderByDescending(a => a.Id));
        }
    }
}
