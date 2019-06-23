using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgosaToolAPI.Models.Db
{
    public class CommentTagSetTag
    {
        public int Id { get; set; }
        // コメントタグセットID
        // public string CommnetTagSetId { get; set; }
        // コメントタグセット
        public virtual CommentTagSet CommentTagSet { get; set; }
        // タグID
        // public string TagId { get; set; }
        // タグID
        public virtual Tag Tag { get; set; }
    }
}