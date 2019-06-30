using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace EgosaToolAPI.Models.Db
{
    public class ApplicationDbContextOptionsFactory
    {
        public static DbContextOptions Get(IConfiguration configuration)
        {
            var db_user = configuration["read-user"];
            var db_password = configuration["read-password"];
            var connectionString = $"Server=tcp:egosatoolapidbserver.database.windows.net,1433;Initial Catalog=egosaToolAPI_db;Persist Security Info=False;User ID={db_user};Password={db_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(connectionString);

            return builder.Options;
        }
    }
}
