using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using Zyzzyva.Database.Tables;

namespace Zyzzyva.Akka.Database.Messages
{
    public class InsertPersona
    {
        public readonly Persona Persona;

        public readonly IActorRef Sender;

        public InsertPersona(Persona persona, IActorRef sender) => (Persona, Sender) = (persona, sender);
    }
}
