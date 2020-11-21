using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using Zyzzyva.Akka.Database.Messages;
using Zyzzyva.Database;
using Zyzzyva.Database.Tables;

namespace Zyzzyva.Akka.Database.Children
{
    

    public class PersonaWriter : ReceiveActor
    {
        private readonly IPersonaCRUD _crud = new PersonaCRUDdb();
        private readonly string _processorId;

        public PersonaWriter(string id)
        {
            _processorId = id;
            Receive<InsertPersona>(msg => msg.Sender.Tell(new InsertPersonaResponse(_crud.InsertPersona(msg.Persona), _processorId)));
            Receive<DeletePersona>(msg => msg.Sender.Tell(new DeletePersonaResponse(_crud.DeletePersona(msg.Id), _processorId)));
            Receive<UpdatePersona>(msg => msg.Sender.Tell(new UpdatePersonaResponse(_crud.UpdatePersona(msg.Persona), _processorId)));
        }

        public static Props MyProps(string id) => Props.Create(() => new PersonaWriter(id));
    }
   
}
