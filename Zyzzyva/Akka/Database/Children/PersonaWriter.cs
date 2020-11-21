﻿using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using Zyzzyva.Akka.Database.Messages;
using Zyzzyva.Database;

namespace Zyzzyva.Akka.Database.Children
{
    

    public class PersonaWriter : ReceiveActor
    {
        private readonly PersonaCRUDdb _crud;
        private readonly string _processorId;

        public PersonaWriter(string id, PersonaCRUDdb cRUDdb)
        {
            _processorId = id;
            _crud = cRUDdb;
            Receive<InsertPersona>(msg => msg.Sender.Tell(new InsertPersonaResponse(_crud.InsertPersona(msg.Persona), _processorId)));
            Receive<DeletePersona>(msg => msg.Sender.Tell(new DeletePersonaResponse(_crud.DeletePersona(msg.Id), _processorId)));
            Receive<UpdatePersona>(msg => msg.Sender.Tell(new UpdatePersonaResponse(_crud.UpdatePersona(msg.Persona), _processorId)));
        }

        public static Props MyProps(string id,PersonaCRUDdb cRUDdb) => Props.Create(() => new PersonaWriter(id,cRUDdb));
    }
   
}
