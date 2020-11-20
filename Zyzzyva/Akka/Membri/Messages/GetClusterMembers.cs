using Akka.Actor;

namespace Zyzzyva.Akka.Membri.Messages
{
    public class GetClusterMembers
    {
        public IActorRef ActorRef1 { get; }

        public GetClusterMembers(IActorRef actorRef) => ActorRef1 = actorRef;
    }
}
