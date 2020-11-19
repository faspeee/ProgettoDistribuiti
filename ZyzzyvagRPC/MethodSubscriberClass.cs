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
        public event EventHandler<MethodUpdateEventArgs2> UpdateMembers;
        private IActorRef actor;

        public MethodSubscriberClass()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }
        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            Console.WriteLine("CHIYSO");
            ClusterClientAccess.KillActor(actor);
        }

        public void GetFibonacci(int number)
        {
            actor = ClusterClientAccess.CreateActor(Dummy.MyProps(Update));
            ClusterClientAccess.Instance.GetFibonacci(number, actor);
        }

        public void GetMembers()
        {
            ClusterClientAccess.Instance.GetMembers(actor);
        }

        public void CreateActor() => actor = ClusterClientAccess.CreateActor(Dummy.MyProps(this, Update, UpdateMembers));

    }

    class Dummy : ReceiveActor
    {

        private event EventHandler<MethodUpdateEventArgs> Update;
        private event EventHandler<MethodUpdateEventArgs2> UpdateM;
        private MethodSubscriberClass LOL;
        //private Stream stream;
        public Dummy(MethodSubscriberClass sender, EventHandler<MethodUpdateEventArgs> u, EventHandler<MethodUpdateEventArgs2> u2)
        {

            Update = u;
            UpdateM = u2;
            LOL = sender;
            Receive<ProcessorResponse>(x => Update?.Invoke(LOL, new MethodUpdateEventArgs(x.Result)));
            Receive<ListMembers>(x => UpdateM?.Invoke(LOL, new MethodUpdateEventArgs2(x.addresses)));
        }

        public static Props MyProps(MethodSubscriberClass sender,EventHandler<MethodUpdateEventArgs> u, EventHandler<MethodUpdateEventArgs2> u2) => Props.Create(() => new Dummy(sender, u,u2));
    }


}
