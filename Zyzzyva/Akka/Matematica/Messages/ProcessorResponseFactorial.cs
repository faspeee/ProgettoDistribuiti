namespace Zyzzyva.Akka.Matematica.Messages
{
    public class ProcessorResponseFactorial
    {
        public int Result { get; }
        public string ProcessorId { get; }

        public ProcessorResponseFactorial(int n, string sender) => (Result, ProcessorId) = (n, sender);

    }
}
