using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using Zyzzyva.Akka.Matematica.Messages;

namespace Zyzzyva.Akka.Matematica
{
    public class MatematicaProcessor : ReceiveActor
    {
        private readonly string _processorId;

        public MatematicaProcessor(string id)
        {
            _processorId = id;
            Receive<ComputeFibonacci>(msg => {
                Console.WriteLine("IO SONO+ " + _processorId + " ARRIVATA RICHIESTA DI CALCOLARE +" + msg.Number + " DAL TIZIO CHIAMATO " + msg.Sender.Path);
                msg.Sender.Tell(new ProcessorResponseFibonacci(fibonacci(msg.Number), _processorId));
            });
            Receive<ComputeFactorial>(msg => {
                Console.WriteLine("IO SONO+ " + _processorId + " ARRIVATA RICHIESTA DI CALCOLARE FACTORIAL+" + msg.Number + " DAL TIZIO CHIAMATO " + msg.Sender.Path);
                msg.Sender.Tell(new ProcessorResponseFactorial(factorial(msg.Number), _processorId));
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

        private int factorial(int x)
        {
            static int factHelper(int xx, int acc = 1) => xx switch
            {
                <= 1 => acc, 
                _ => factHelper(xx - 1, xx * acc)
            };

            return factHelper(x);
        }
       
        public static Props MyProps(string processorId)
        {
            return Props.Create(() => new MatematicaProcessor(processorId));
        }

    }
}
