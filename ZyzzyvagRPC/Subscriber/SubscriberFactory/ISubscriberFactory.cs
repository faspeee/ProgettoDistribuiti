using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZyzzyvagRPC.Subscriber.SubscriberContract;

namespace ZyzzyvagRPC.Checazzonesoio
{
    public interface ISubscriberFactory
    {
        IFibonacciSubscriber GetFibonacciSubscriber();
        IMemberSubscriber GetMemberSubscriber();
    }
}
