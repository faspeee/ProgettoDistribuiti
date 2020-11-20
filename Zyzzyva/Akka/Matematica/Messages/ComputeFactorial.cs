using Akka.Actor;

namespace Zyzzyva.Akka.Matematica.Messages
{
    public class ComputeFactorial
    {
        public int Number { get; }

        public IActorRef Sender { get; }
        public ComputeFactorial(int n, IActorRef actorRef1) => (Number, Sender) = (n, actorRef1);
    }
}
