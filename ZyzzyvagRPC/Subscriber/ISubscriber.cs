using System;

namespace ZyzzyvagRPC.Subscriber
{
    public interface ISubscriber:IDisposable
    {
        void CreateActor();
    }
}
