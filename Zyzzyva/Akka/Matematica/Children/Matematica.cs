using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using Zyzzyva.Akka.Matematica.Messages;

namespace Zyzzyva.Akka.Matematica
{
    public class Matematica : ReceiveActor
    {
        private readonly IActorRef matematicaProcessor;
        public Matematica(string id)
        {
            matematicaProcessor = Context.ActorOf(MatematicaProcessor.MyProps(id), "matematicaProcessor");
            Receive<ComputeFibonacci>(msg => matematicaProcessor.Tell(msg));
            Receive<ComputeFactorial>(msg => matematicaProcessor.Tell(msg));
        }

        public static Props MyProps(string id) => Props.Create(() => new Matematica(id));
    }
}
