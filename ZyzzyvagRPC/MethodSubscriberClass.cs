using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZyzzyvagRPC.Checazzonesoio;
using ZyzzyvaRPC.ClusterClientAccess;
using static Zyzzyva.src.Main.Akka.Core.ClusterManager;
using static Zyzzyva.src.Main.Akka.Core.ProcessorFibonacci;

namespace ZyzzyvagRPC
{
    public class MethodSubscriberClass : IMethodSubscriber
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Task _task;
        public event EventHandler<MethodUpdateEventArgs> Update;
        private IActorRef actor;

        public MethodSubscriberClass()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }
        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        public void GetFibonacci(int number)
        {
            actor = ClusterClientAccess.CreateActor(Dummy.MyProps(this,Update));
            ClusterClientAccess.Instance.GetFibonacci(number, actor);
        }

        public void GetMembers()
        {
            ClusterClientAccess.Instance.GetMembers(actor);
        }
    }

    class Dummy : ReceiveActor
    {

        private event EventHandler<MethodUpdateEventArgs> Update;
        private MethodSubscriberClass LOL;
        //private Stream stream;
        public Dummy(MethodSubscriberClass sender, EventHandler<MethodUpdateEventArgs> u)
        {

            Update = u;
            LOL = sender;
            Receive<ProcessorResponse>(x => Update?.Invoke(LOL, new MethodUpdateEventArgs(x.Result)));
            Receive<ListMembers>(x => {
                Console.WriteLine("ARRIVATA RISPOSTA!");
                x.addresses.ForEach(xx => Console.WriteLine("Ci SONO + " + xx));
                //stream.Streamma(xx);

            });
        }

        public static Props MyProps(MethodSubscriberClass sender,EventHandler<MethodUpdateEventArgs> u) => Props.Create(() => new Dummy(sender, u));
    }


}
