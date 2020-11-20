namespace Zyzzyva.Akka.Matematica.Messages
{
    public class ProcessorResponseFibonacci
    {
        public int Result { get; }
        public string ProcessorId { get; }

        public ProcessorResponseFibonacci(int n, string sender) => (Result, ProcessorId) = (n, sender);

    }
}
