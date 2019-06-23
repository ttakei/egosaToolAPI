using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Threading.Tasks;

namespace EgosaToolAPI.Models.Db
{
    public class CommentsContext : DbContext
    {

        public DbSet<Comment> Comments { get; set; }

        public CommentsContext() : base("MyDatabase2")
        {
        }
    }
}