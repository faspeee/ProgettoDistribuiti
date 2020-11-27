using Akka.Actor;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using Zyzzyva.Akka.Matematica.Messages;

namespace Zyzzyva.Akka.Matematica
{
    public class MatematicaManagerActor : ReceiveActor
    {

        private readonly IActorRef _matematicaRouter;

        private MatematicaManagerActor(string id)
        {
            _matematicaRouter = Context.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "matematicaRouter");

            Context.ActorOf(Matematica.MyProps(Self.Path.Address.Port+id), "matematica");

            Receive<GetFibonacci>(value => _matematicaRouter.Forward(new ComputeFibonacci(value.Number,value.ActorRef1)));
            Receive<GetFactorial>(value => _matematicaRouter.Forward(new ComputeFactorial(value.Number, value.ActorRef1)));
        }

        public static Props MyProps(string id) => Props.Create(() => new MatematicaManagerActor(id));

    }
}