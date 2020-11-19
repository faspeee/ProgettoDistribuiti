using Akka.Actor;
using System;
using System.Threading;
using ZyzzyvaRPC.ClusterClientAccess;

namespace ZyzzyvagRPC.Subscriber
{
    public abstract class AbstractSubscriber : ISubscriber
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        protected IActorRef _actor; 
        public abstract void CreateActor();
        public AbstractSubscriber() {
            _cancellationTokenSource = new CancellationTokenSource();
        } 
        public void Dispose()
        { 
            ClusterClientAccess.KillActor(_actor);
            _cancellationTokenSource.Cancel(); 
        }
    }
     
}
