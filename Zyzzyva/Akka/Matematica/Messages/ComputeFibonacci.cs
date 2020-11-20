using Akka.Actor;

namespace Zyzzyva.Akka.Matematica.Messages
{
    public class ComputeFibonacci
    {
        public int Number { get; }

        public IActorRef Sender { get; }
        public ComputeFibonacci(int n, IActorRef actorRef1) => (Number, Sender) = (n, actorRef1);
    }
}

