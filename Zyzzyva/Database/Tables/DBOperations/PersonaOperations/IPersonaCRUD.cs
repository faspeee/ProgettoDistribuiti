using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Zyzzyva.Database.Tables
{
    public interface IPersonaCRUD
    {
        public ImmutableList<Persona> ReadAllPersone();
        public Persona ReadPersona(int id);
        public ImmutableList<Persona> InsertPersona(Persona persona);
        public ImmutableList<Persona> DeletePersona(int id);
        public ImmutableList<Persona> UpdatePersona(Persona persona);
    }
}
