using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EgosaToolAPI.Models.Db
{
    public class CommentTagSetsContext : DbContext
    {
        public DbSet<CommentTagSet> CommentTagSets { get; set; }
    }
}