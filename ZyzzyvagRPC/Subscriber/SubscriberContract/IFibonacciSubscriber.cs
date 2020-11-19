using System;
using ZyzzyvagRPC.ZyzzyvaImplementation.EventArgument;

namespace ZyzzyvagRPC.Subscriber.SubscriberContract
{
    public interface IFibonacciSubscriber : ISubscriber
    {
        event EventHandler<FibonacciEventArgs> FibonacciEvent;
        void GetFibonacci(int number);
    }
}
