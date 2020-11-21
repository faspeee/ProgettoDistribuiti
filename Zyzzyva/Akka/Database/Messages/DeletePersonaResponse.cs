using System.Collections.Immutable;
using Zyzzyva.Database.Tables;

namespace Zyzzyva.Akka.Database.Messages
{
    public class DeletePersonaResponse
    {
        public readonly ImmutableList<Persona> Personas;
        public readonly string ProcessorId;

        public DeletePersonaResponse(ImmutableList<Persona> personas, string processorId) => (Personas, ProcessorId) = (personas, processorId);
        
    }
}