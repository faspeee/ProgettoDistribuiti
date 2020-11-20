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
        public event EventHandler<FibonacciEventArgs> FibonacciEvent;
        public event EventHandler<FactorialEventArgs> FactorialEvent;

        public override void CreateActor() => _actor = ClusterClientAccess.CreateActor(FibonacciActor.MyProps(this, FibonacciEvent,FactorialEvent));

        public void GetFactorial(int number) => ClusterClientAccess.Instance.GetFactorial(number, _actor);
       
        public void GetFibonacci(int number)=> ClusterClientAccess.Instance.GetFibonacci(number, _actor);

        private class FibonacciActor : ReceiveActor
        { 
            private event EventHandler<FibonacciEventArgs> FibonaciEvent;
            private event EventHandler<FactorialEventArgs> FactorialEvent;
            private readonly FibonacciSubscriber FibonacciSubscriber;
            //private Stream stream;
            public FibonacciActor(FibonacciSubscriber fibonacci, EventHandler<FibonacciEventArgs> fibonacciEvent, EventHandler<FactorialEventArgs> factorialEvent)
            {

                FibonaciEvent = fibonacciEvent; 
                FibonacciSubscriber = fibonacci;
                FactorialEvent = factorialEvent;
                Receive<ProcessorResponse>(x => FibonaciEvent?.Invoke(FibonacciSubscriber, new FibonacciEventArgs(x.Result)));
                Receive<ProcessorResponseFactorial>(x => FactorialEvent?.Invoke(FibonacciSubscriber, new FactorialEventArgs(x.Result)));
            }

            public static Props MyProps(FibonacciSubscriber fibonacci, EventHandler<FibonacciEventArgs> fibonacciEvent, EventHandler<FactorialEventArgs> factorialEvent) => Props.Create(() => new FibonacciActor(fibonacci, fibonacciEvent,factorialEvent));
        }
    }

    


}
