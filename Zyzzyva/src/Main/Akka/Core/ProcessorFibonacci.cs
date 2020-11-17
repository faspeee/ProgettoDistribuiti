using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zyzzyva.src.Main.Akka.Core
{
    class ProcessorFibonacci : ReceiveActor
    {
        private readonly string _processorId;

        public ProcessorFibonacci(string id)
        {
            _processorId = id;
            Receive<ComputeMessage>(msg => {
                Console.WriteLine("IO SONO+ " + _processorId + " ARRIVATA RICHIESTA DI CALCOLARE +" + msg.Number + " DAL TIZIO CHIAMATO " + msg.Sender.Path);
                msg.Sender.Tell(new ProcessorResponse(fibonacci(msg.Number), _processorId));
            });


        }
        private int fibonacci(int x)
        {
            static int fibHelper(int xx, int prev = 0, int next = 1) => xx switch
            {
                0 => prev,
                1 => next,
                _ => fibHelper(xx - 1, next, next + prev)
            };

            return fibHelper(x);
        }


        public class ComputeMessage
        {
            public int Number { get; }
            public IActorRef Sender { get; }

            public ComputeMessage(int n, IActorRef sender) => (Number,Sender) = (n, sender);

        }

        public class ProcessorResponse
        {
            public int Result { get; }
            public string ProcessorId { get; }

            public ProcessorResponse(int n, string sender) => (Result, ProcessorId) = (n, sender);

        }

        public static Props MyProps(string processorId)
        {
            return Props.Create(() => new ProcessorFibonacci(processorId));
        }

    }
}
