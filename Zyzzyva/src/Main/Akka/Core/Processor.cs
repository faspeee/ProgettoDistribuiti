using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zyzzyva.src.Main.Akka.Core
{
    public class Processor : ReceiveActor
    {
        private IActorRef fibonacciProcessor;
        public Processor(string id)
        {
            fibonacciProcessor = Context.ActorOf(ProcessorFibonacci.MyProps(id), "Fibonacci");
            Receive<ComputeFibonacci>(msg => fibonacciProcessor.
                                             Tell(new ProcessorFibonacci.ComputeMessage(msg.Number, msg.ActorRef1)));
        }

        public class ComputeFibonacci
        {
            public int Number { get; }

            public IActorRef ActorRef1 { get; }
            public ComputeFibonacci(int n, IActorRef actorRef1) => (Number, ActorRef1) = (n, actorRef1);
        }

        public static Props MyProps(string id) => Props.Create(() => new Processor(id));
    }
}
