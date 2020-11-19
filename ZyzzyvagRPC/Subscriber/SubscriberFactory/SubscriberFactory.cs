using ZyzzyvagRPC.Subscriber.SubscriberContract;
using ZyzzyvagRPC.Subscriber.SubscriberImplementation;

namespace ZyzzyvagRPC.Checazzonesoio
{
    public class SubscriberFactory : ISubscriberFactory
    {
        public IFibonacciSubscriber GetFibonacciSubscriber() => new FibonacciSubscriber();
        
        public IMemberSubscriber GetMemberSubscriber() => new MemberSubscriber();
        
    }
}
