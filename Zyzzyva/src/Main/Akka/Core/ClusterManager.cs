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
        private readonly Cluster _cluster = Cluster.Get(Context.System);
        public ClusterManager(string id)
        {
            Context.ActorOf(ClusterListener.MyProps(id,_cluster ),"clusterListener");

            Receive<GetMembers>(_ => Sender.Tell(new ListMembers(_cluster.State.Members.Where(x => x.Status == MemberStatus.Up).Select(xx => xx.Address.ToString()).ToList())));
        }

        public static Props MyProps(string id) => Props.Create(() => new ClusterManager(id));

        public class GetMembers { };

        public class ListMembers
        {
            public List<string> addresses { get; }

            public ListMembers(List<string> members) => addresses = members;
        }


    }
}