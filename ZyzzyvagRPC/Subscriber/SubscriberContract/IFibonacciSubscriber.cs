using System;
using ZyzzyvagRPC.Subscriber.EventArgument;
using ZyzzyvagRPC.ZyzzyvaImplementation.EventArgument;

namespace ZyzzyvagRPC.Subscriber.SubscriberContract
{
    /// <include file="../../Docs/Subscriber/SubscriberContract/IFibonacciSubscriber.xml" path='docs/members[@name="ifibonaccisubscriber"]/IFibonacciSubscriber/*'/> 
    public interface IFibonacciSubscriber : ISubscriber
    {
        event EventHandler<FibonacciEventArgs> FibonacciEvent;
        event EventHandler<FactorialEventArgs> FactorialEvent;

        /// <include file="../../Docs/Subscriber/SubscriberContract/IFibonacciSubscriber.xml" path='docs/members[@name="ifibonaccisubscriber"]/GetFibonacci/*'/> 
        void GetFibonacci(int number);

        /// <include file='../../Docs/Subscriber/SubscriberContract/IFibonacciSubscriber.xml' path='docs/members[@name="ifibonaccisubscriber"]/GetFactorial/*'/> 
        void GetFactorial(int number);
    }
}
