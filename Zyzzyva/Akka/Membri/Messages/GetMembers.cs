using Akka.Actor;

namespace Zyzzyva.Akka.Membri.Messages
{
    public class GetMembers
    {
        public IActorRef ActorRef1 { get; }

        public GetMembers(IActorRef actorRef) => ActorRef1 = actorRef;
    };
}
