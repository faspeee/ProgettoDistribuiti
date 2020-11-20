
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Zyzzyva.Database.Tables;

namespace Zyzzyva.Database.Settings
{
    public class SQLiteDatabaseContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source="+ConfigurationManager.AppSettings["dbpath"]);

        public DbSet<Persona> Persona { get; set; }
        
    }
}
