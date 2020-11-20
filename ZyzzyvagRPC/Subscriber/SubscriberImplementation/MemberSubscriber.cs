using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZyzzyvagRPC.Subscriber.SubscriberContract;
using ZyzzyvagRPC.ZyzzyvaImplementation.EventArgument;
using ZyzzyvaRPC.ClusterClientAccess;
using Akka.Actor;
using static Zyzzyva.src.Main.Akka.Core.ClusterManager;

namespace ZyzzyvagRPC.Subscriber.SubscriberImplementation
{
    public class MemberSubscriber : AbstractSubscriber,IMemberSubscriber 
    {
        public event EventHandler<MemberEventArgs> MemberEvent;
        public override void CreateActor() => _actor = ClusterClientAccess.CreateActor(MemberActor.MyProps(this, MemberEvent));
        public void GetMembers()
        {
            ClusterClientAccess.Instance.GetMembers(_actor);
        }
        private class MemberActor : ReceiveActor
        { 
            private event EventHandler<MemberEventArgs> MemberEvent;
            private readonly MemberSubscriber MemberSubscriber;
            //private Stream stream;
            public MemberActor(MemberSubscriber member, EventHandler<MemberEventArgs> memberEvent)
            {

                MemberEvent = memberEvent;
                MemberSubscriber = member; 
                Receive<ListMembers>(x => 
                MemberEvent?.Invoke(MemberSubscriber, new MemberEventArgs(x.addresses))
                );
            }

            public static Props MyProps(MemberSubscriber member, EventHandler<MemberEventArgs> memberEvent) => Props.Create(() => new MemberActor(member, memberEvent));
        }
    }


}
