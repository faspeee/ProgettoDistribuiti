using System.Collections.Immutable;
using Zyzzyva.Database.Tables;

namespace Zyzzyva.Akka.Database.Children
{
    public class UpdatePersonaResponse
    {
        public readonly ImmutableList<Persona> Persona;
        public readonly string ProcessorId;

        public UpdatePersonaResponse(ImmutableList<Persona> persona, string processorId) => (Persona, ProcessorId) = (persona, processorId);
        
    }
}