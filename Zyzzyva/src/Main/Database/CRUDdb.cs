using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zyzzyva.src.Main.Database.Settings;
using Zyzzyva.src.Main.Database.Tables;

namespace Zyzzyva.src.Main.Database
{
    class CRUDdb
    {
        private SQLiteDatabaseContext db;
        public CRUDdb(string pathDB)
        {
            db = new SQLiteDatabaseContext();
        }

        public List<Ordini> readOrdini()
        {
            return db.Ordini.Select(x => x).ToList();
        }

    }
}
