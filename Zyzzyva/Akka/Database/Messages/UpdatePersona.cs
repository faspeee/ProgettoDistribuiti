using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using Zyzzyva.Database.Tables;

namespace Zyzzyva.Akka.Database.Messages
{
    public class UpdatePersona 
    {
        public readonly Persona Persona;

        public readonly IActorRef Sender;

        public UpdatePersona(Persona persona, IActorRef sender) => (Persona, Sender) = (persona, sender);
    }
}
