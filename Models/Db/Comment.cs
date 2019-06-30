using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EgosaToolAPI.Models.Db
{
    public class Comment
    {
        public int Id { get; set; }

        [ForeignKey("CommentTagSet")]
        public int CommentTagSetId { get; set; }
        public CommentTagSet CommentTagSet { get; set; }

        public string Source { get; set; }

        [Index]
        public string SourceCommentId { get; set; }

        public DateTime PostAt { get; set; }

        public string Body { get; set; }

        public DateTime SearchedAt { get; set; }

        public string SearchWord { get; set; }
    }
}