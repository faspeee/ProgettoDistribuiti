using System;
using ZyzzyvagRPC.Subscriber.EventArgument;
using ZyzzyvagRPC.ZyzzyvaImplementation.EventArgument;

namespace ZyzzyvagRPC.Subscriber.SubscriberContract
{
    public interface IFibonacciSubscriber : ISubscriber
    {
        event EventHandler<FibonacciEventArgs> FibonacciEvent;
        event EventHandler<FactorialEventArgs> FactorialEvent;
        void GetFibonacci(int number);
        void GetFactorial(int number);
    }
}
