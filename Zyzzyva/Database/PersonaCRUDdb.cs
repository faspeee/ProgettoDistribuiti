using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Zyzzyva.Database.Settings;
using Zyzzyva.Database.Tables;

namespace Zyzzyva.Database
{
    public class PersonaCRUDdb
    {
        private SQLiteDatabaseContext db;
        public PersonaCRUDdb()
        {
            db = new SQLiteDatabaseContext();
        }

        public ImmutableList<Persona> ReadAllPersone() => db.Persona.Select(x => x).ToImmutableList();

        public Persona ReadPersona(int id) => FindPersona(id);


        public int InsertPersona(Persona persona)
        {
            db.Add(persona);
            db.SaveChanges();
            return persona.id;
        }

        public int DeletePersona(int id)
        {
            var pers = FindPersona(id);
            db.Remove(pers);
            db.SaveChanges();
            return id;
        }

        public int UpdatePersona(Persona persona)
        {
            var x = FindPersona(persona.id);
            Persona.ModifyPersona(x, persona);
            db.SaveChanges();
            return persona.id;

        }

        private Persona FindPersona(int id) => db.Persona.Where(x => x.id == id).First();


    }
}
