
using Microsoft.EntityFrameworkCore;
using System.Configuration; 
using Zyzzyva.Database.Tables; 

namespace Zyzzyva.Database.Settings
{
    public class SQLiteDatabaseContext : DbContext
    {

        private static readonly string PATH = "~/../../Zyzzyva/Database/Settings/dbconfig.hocon";
        private static readonly string PATH2 = "config/dbconfig.hocon";

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var congi = Hocon.HoconConfigurationFactory.FromFile(PATH2);
            options.UseSqlite("Data Source=" + congi.GetString("dbpath.path"));
        } 

        public DbSet<Persona> Persona { get; set; }
        
    }
}
