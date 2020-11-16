using Akka.Actor;
using Akka.Cluster;
using Akka.Event;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Zyzzyva.src.Main.Akka.Core
{
    class ClusterManager : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Logging.GetLogger(Context);
        private readonly Cluster _cluster = Cluster.Get(Context.System);
        private readonly IActorRef _listener;
        ClusterManager(string id)
        {
            _listener = Context.ActorOf(ClusterListener.MyProps(id,_cluster ),"clusterListener");

            Receive<GetMembers>(_ => Sender.Tell(_cluster.State.Members.Where(x => x.Status == MemberStatus.Up).Select(xx => xx.Address.ToString()).ToList()));
        }

        public static Props MyProps(string id) => Props.Create(() => new ClusterManager(id));

        public record GetMembers;


    }
}