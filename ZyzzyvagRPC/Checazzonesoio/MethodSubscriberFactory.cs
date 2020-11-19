using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZyzzyvagRPC.Checazzonesoio
{
    public class MethodSubscriberFactory : IMethodSubscriberFactory
    {
        public IMethodSubscriber GetSubscriber() => new MethodSubscriberClass();

    }
}
