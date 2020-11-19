using Akka.Actor;
using System;
using ZyzzyvagRPC.Subscriber.SubscriberContract;
using ZyzzyvagRPC.ZyzzyvaImplementation.EventArgument;
using ZyzzyvaRPC.ClusterClientAccess;
using static Zyzzyva.src.Main.Akka.Core.ProcessorFibonacci;

namespace ZyzzyvagRPC.Subscriber.SubscriberImplementation
{
    public class FibonacciSubscriber : AbstractSubscriber, IFibonacciSubscriber
    { 
        public event EventHandler<FibonacciEventArgs> FibonacciEvent;
         
        public override void CreateActor() => _actor = ClusterClientAccess.CreateActor(FibonacciActor.MyProps(this, FibonacciEvent));
        public void GetFibonacci(int number)=> ClusterClientAccess.Instance.GetFibonacci(number, _actor);

        private class FibonacciActor : ReceiveActor
        { 
            private event EventHandler<FibonacciEventArgs> FibonaciEvent;
            private readonly FibonacciSubscriber FibonacciSubscriber;
            //private Stream stream;
            public FibonacciActor(FibonacciSubscriber fibonacci, EventHandler<FibonacciEventArgs> fibonacciEvent)
            {

                FibonaciEvent = fibonacciEvent; 
                FibonacciSubscriber = fibonacci;
                Receive<ProcessorResponse>(x => FibonaciEvent?.Invoke(FibonacciSubscriber, new FibonacciEventArgs(x.Result))); 
            }

            public static Props MyProps(FibonacciSubscriber fibonacci, EventHandler<FibonacciEventArgs> fibonacciEvent) => Props.Create(() => new FibonacciActor(fibonacci, fibonacciEvent));
        }
    }

    


}
