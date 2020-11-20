namespace Zyzzyva.Akka.Database.Messages
{
    public class DeletePersonaResponse
    {
        public readonly int Mistero;
        public string ProcessorId;

        public DeletePersonaResponse(int v, string processorId) => (Mistero, ProcessorId) = (v, processorId);
        
    }
}