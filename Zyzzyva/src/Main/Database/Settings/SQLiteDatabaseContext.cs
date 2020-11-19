
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Zyzzyva.src.Main.Database.Tables;

namespace Zyzzyva.src.Main.Database.Settings
{
    public class SQLiteDatabaseContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source="+ConfigurationManager.AppSettings["dbpath"]);

        public DbSet<Ordini> Ordini { get; set; }
        
    }
}
