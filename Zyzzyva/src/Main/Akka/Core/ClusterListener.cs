using Akka.Actor;
using Akka.Cluster;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.Text;
using static Akka.Cluster.ClusterEvent;

namespace Zyzzyva.src.Main.Akka.Core
{
    class ClusterListener : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Logging.GetLogger(Context);
        private readonly Cluster _cluster;
        private readonly string _id;
        protected override void PreStart()
        {
            _cluster.Subscribe(Self, SubscriptionInitialStateMode.InitialStateAsEvents,
                typeof(IMemberEvent), typeof(UnreachableMember));
        }

        protected override void PostStop() => _cluster.Unsubscribe(Self);

        public ClusterListener(string id, Cluster cluster)
        {
            _cluster = cluster;
            _id = id;

            Receive<MemberUp>(member => _log.Debug($"Node {_id} - Member is Up: {member.Member.Address}"));
            Receive<UnreachableMember>(member => _log.Debug($"Node {_id} - Member detected as unreachable: {member.Member.Address}"));
            Receive<MemberRemoved>(member => _log.Debug($"Node {_id} - Member is Removed: {member.Member.Address} after {member.PreviousStatus }"));

        }

        public static Props MyProps(string nodeID, Cluster cluster) => Props.Create(() => new ClusterListener(nodeID, cluster));
    }
}