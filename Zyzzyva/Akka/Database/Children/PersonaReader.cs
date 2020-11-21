using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using Zyzzyva.Akka.Database.Messages;
using Zyzzyva.Database;
using Zyzzyva.Database.Tables;

namespace Zyzzyva.Akka.Database.Children
{
    public class PersonaReader : ReceiveActor
    {

        private readonly IPersonaCRUD _crud = new PersonaCRUDdb();
        private readonly string _processorId;

        public PersonaReader(string id)
        {
            _processorId = id;
            Receive<ReadAllPersona>(msg => msg.Sender.Tell(new ReadAllPersonaResponse(_crud.ReadAllPersone(), _processorId)));
            Receive<ReadPersona>(msg => msg.Sender.Tell(new ReadPersonaResponse(_crud.ReadPersona(msg.Id), _processorId)));
        }

        public static Props MyProps(string id ) => Props.Create(() => new PersonaReader(id));
    }
}
