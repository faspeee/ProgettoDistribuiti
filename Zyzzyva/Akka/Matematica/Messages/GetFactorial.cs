using Akka.Actor;

namespace Zyzzyva.Akka.Matematica.Messages
{
    public class GetFactorial
    {
        public int Number { get; }
        public IActorRef ActorRef1 { get; }

        public GetFactorial(int n, IActorRef actorRef) => (Number, ActorRef1) = (n, actorRef);
    }
}
