using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zyzzyva.Akka.Database.Messages
{
    public class ReadPersona
    {
        public readonly int Id;

        public readonly IActorRef Sender;

        public ReadPersona(int id, IActorRef sender) => (Id, Sender) = (id, sender);
    }
}
