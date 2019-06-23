using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EgosaToolAPI.Models.Db
{
    public class Comment
    {
        public int Id { get; set; }
        // コメントとタグの紐づけ
        public int CommentTagSetId { get; set; }
        // public virtual CommentTagSet CommentTagSet { get; set; }
        // コメント元
        public string Source { get; set; }
        // コメント元ID
        [Index]
        public string SourceCommentId { get; set; }
        // 投稿日時
        public DateTime PostAt { get; set; }
        // 投稿内容
        public string Body { get; set; }
        // 検索日時
        public DateTime SearchedAt { get; set; }
        // 検索単語
        public string SearchWord { get; set; }
    }
}