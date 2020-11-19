using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZyzzyvagRPC.Checazzonesoio
{
    public interface IMethodSubscriberFactory
    {
        IMethodSubscriber GetSubscriber();
    }
}
