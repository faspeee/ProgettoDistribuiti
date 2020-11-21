using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Zyzzyva.Database.Settings;
using Zyzzyva.Database.Tables;

namespace Zyzzyva.Database
{

    public class PersonaCRUDdb : SQLiteDatabaseContext, IPersonaCRUD
    {
        private SQLiteDatabaseContext db;
        public PersonaCRUDdb() => db = new SQLiteDatabaseContext();

        public ImmutableList<Persona> ReadAllPersone()
        {
            var x = db.Persona.Select(x => x).ToImmutableList();
            return x;
        }

        public Persona ReadPersona(int id) => FindPersona(id);


        public ImmutableList<Persona> InsertPersona(Persona persona)
        {
            db.Add(persona);
            db.SaveChanges();
            return db.Persona.ToImmutableList();
        }

        public ImmutableList<Persona> DeletePersona(int id)
        {
            var pers = FindPersona(id);
            db.Remove(pers);
            db.SaveChanges();
            return db.Persona.ToImmutableList();
        }

        public ImmutableList<Persona> UpdatePersona(Persona persona)
        {
            var x = FindPersona(persona.id);
            Tables.Persona.ModifyPersona(x, persona);
            db.SaveChanges();
            return db.Persona.ToImmutableList();

        }

        private Persona FindPersona(int id) => db.Persona.Where(x => x.id == id).First();


    }
}
