using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using Zyzzyva.Akka.Database.Messages;
using Zyzzyva.Database;

namespace Zyzzyva.Akka.Database.Children
{
    public class PersonaActor : ReceiveActor
    {
        private readonly PersonaCRUDdb _cRUDdb;
        private readonly IActorRef _personaWriter;
        private readonly IActorRef _personaReader;

        public PersonaActor(string id, PersonaCRUDdb cRUDdb)
        {
            _cRUDdb = cRUDdb;
            _personaReader = Context.ActorOf(PersonaReader.MyProps(id, _cRUDdb), "personaReader");
            _personaWriter = Context.ActorOf(PersonaWriter.MyProps(id,_cRUDdb), "personaWriter");

            Receive<ReadPersona>(msg => _personaReader.Tell(msg));
            Receive<ReadAllPersona>(msg => _personaReader.Tell(msg));
            
            Receive<DeletePersona>(msg => _personaWriter.Tell(msg));
            Receive<UpdatePersona>(msg => _personaWriter.Tell(msg));
            Receive<InsertPersona>(msg => _personaWriter.Tell(msg));
        }

        public static Props MyProps(string id, PersonaCRUDdb cRUDdb) => Props.Create(() => new PersonaActor(id,cRUDdb));


    }
}
