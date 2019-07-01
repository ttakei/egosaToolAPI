using EgosaToolAPI.Models.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<ActionResult<string>> Get()
        {
            // 全コメントの取得
            var comments = await db.Comments.OrderByDescending(a => a.Id).ToListAsync();
            return JsonConvert.SerializeObject(comments);
        }
    }
}
