using Akka.Actor;
using Akka.Cluster;
using System.Linq;
using Zyzzyva.Akka.Membri.Messages;

namespace Zyzzyva.Akka.Membri.Children
{
    public class Members : ReceiveActor
    {

        private readonly Cluster _cluster;

        public Members(string id, Cluster cluster)
        {
            _cluster = cluster;
            
            Context.ActorOf(ClusterListener.MyProps(id, cluster),"clusterListener");
            
            Receive<GetMembers>(msg => msg.ActorRef1.Tell(new ListMembers(_cluster.State.Members.Where(x => x.Status == MemberStatus.Up).Select(xx => xx.Address.ToString()).ToList())));
        }

        public static Props MyProps(string id, Cluster cluster) => Props.Create(() => new Members(id, cluster));

        

       


    }
}