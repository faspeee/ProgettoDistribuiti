using Akka.Actor;

namespace Zyzzyva.Akka.Matematica.Messages
{
    public class GetFibonacci
    {
        public int Number { get; }
        public IActorRef ActorRef1 { get; }

        public GetFibonacci(int n, IActorRef actorRef) => (Number, ActorRef1) = (n, actorRef);
    }

}
