using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zyzzyva.src.Main.Akka.Core
{
    class Processor : ReceiveActor
    {
        private IActorRef fibonacciProcessor;
        public Processor(string id)
        {
            fibonacciProcessor = Context.ActorOf(ProcessorFibonacci.MyProps(id), "Fibonacci");
            Receive<ComputeFibonacci>(msg => fibonacciProcessor.
                                             Tell(new ProcessorFibonacci.ComputeMessage(msg.Number, Sender)));
        }

        public class ComputeFibonacci
        {
            public int Number { get; }
            public ComputeFibonacci(int n) => Number = n;
        }

        public static Props MyProps(string id) => Props.Create(() => new Processor(id));
    }
}
