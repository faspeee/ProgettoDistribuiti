namespace Zyzzyva.Akka.Database.Children
{
    internal class UpdatePersonaResponse
    {
        public readonly int Mistero;
        public string ProcessorId;

        public UpdatePersonaResponse(int v, string processorId) => (Mistero, ProcessorId) = (v, processorId);
        
    }
}