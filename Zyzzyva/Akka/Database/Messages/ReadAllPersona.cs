using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using Zyzzyva.Database.Tables;

namespace Zyzzyva.Akka.Database.Messages
{
    public class ReadAllPersona
    {
        public readonly IActorRef Sender;
        public ReadAllPersona(IActorRef sender) => Sender = sender;
    }
}
