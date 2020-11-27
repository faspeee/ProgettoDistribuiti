using Akka.Actor;
using Akka.Cluster;
using Akka.Routing;
using Zyzzyva.Akka.Membri.Children;
using Zyzzyva.Akka.Membri.Messages;

namespace Zyzzyva.Akka.Membri
{
    /// <include file="../../Docs/Akka/Membri/MembriManagerActor.xml" path='docs/members[@name="membrimanager"]/MembriManagerActor/*'/>
    public class MembriManagerActor : ReceiveActor
    {
        private readonly IActorRef _membriRouter;

        private MembriManagerActor(string id)
        {

            _membriRouter = Context.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "memberRouter");

            Context.ActorOf(Members.MyProps(Self.Path.Address.Port + id, Cluster.Get(Context.System)), "members");

            Receive<GetClusterMembers>(msg => _membriRouter.Forward(new GetMembers(msg.ActorRef1)));

        }

        /// <include file="../../Docs/Akka/Membri/MembriManagerActor.xml" path='docs/members[@name="membrimanager"]/MyProps/*'/>

        public static Props MyProps(string id) => Props.Create(() => new MembriManagerActor(id));
    }
}
