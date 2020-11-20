using Akka.Actor;
using System;
using ZyzzyvagRPC.Subscriber.EventArgument;
using ZyzzyvagRPC.Subscriber.SubscriberContract;
using ZyzzyvagRPC.ZyzzyvaImplementation.EventArgument;
using ZyzzyvaRPC.ClusterClientAccess;
using static Zyzzyva.src.Main.Akka.Core.ProcessorFibonacci;

namespace ZyzzyvagRPC.Subscriber.SubscriberImplementation
{
    public class FibonacciSubscriber : AbstractSubscriber, IFibonacciSubscriber
    {
        public event EventHandler Event;

        public override void CreateActor() => _actor = ClusterClientAccess.CreateActor(FibonacciActor.MyProps(this, Event));

        public void GetFactorial(int number) => ClusterClientAccess.Instance.GetFactorial(number, _actor);
       
        public void GetFibonacci(int number)=> ClusterClientAccess.Instance.GetFibonacci(number, _actor);

        private class FibonacciActor : ReceiveActor
        { 
            private event EventHandler Event;
            private readonly FibonacciSubscriber FibonacciSubscriber;
            //private Stream stream;
            public FibonacciActor(FibonacciSubscriber fibonacci, EventHandler evento)
            {

                FibonacciSubscriber = fibonacci;
                Event = evento;
                Receive<ProcessorResponse>(x => Event?.Invoke(FibonacciSubscriber, new FibonacciEventArgs(x.Result)));
                Receive<ProcessorResponseFactorial>(x => Event?.Invoke(FibonacciSubscriber, new FactorialEventArgs(x.Result)));
            }

            public static Props MyProps(FibonacciSubscriber fibonacci, EventHandler Event) => Props.Create(() => new FibonacciActor(fibonacci, Event));
        }
    }

    


}
