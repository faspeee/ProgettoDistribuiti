using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Zyzzyva.Database.Tables
{
    public class Persona
    {

        public static Persona ModifyPersona(Persona orig, Persona persona)
        {
            orig.nome = persona.nome;
            orig.cognome = persona.cognome;
            orig.eta = persona.eta;
            orig.haMacchina = persona.haMacchina;
            return orig;
        }

        public int id { get; set; } 
        public string nome { get; set; }
        public string cognome { get; set; }
        public int eta { get; set; }
        public bool haMacchina { get; set; }
    }
}
