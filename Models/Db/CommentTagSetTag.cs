using System.ComponentModel.DataAnnotations.Schema;

namespace EgosaToolAPI.Models.Db
{
    public class CommentTagSetTag
    {
        public int Id { get; set; }

        [ForeignKey("CommentTagSet")]
        public int CommentTagSetId { get; set; }
        public CommentTagSet CommentTagSet { get; set; }

        [ForeignKey("Tag")]
        public string TagId { get; set; }
        public Tag Tag { get; set; }
    }
}