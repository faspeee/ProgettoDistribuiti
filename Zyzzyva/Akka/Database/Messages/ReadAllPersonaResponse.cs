using System.Collections.Generic;
using System.Collections.Immutable;
using Zyzzyva.Database.Tables;

namespace Zyzzyva.Akka.Database.Children
{
    public class ReadAllPersonaResponse
    {

        public readonly ImmutableList<Persona> Lists;
        public readonly string ProcessorId;

        public ReadAllPersonaResponse(ImmutableList<Persona> lists, string processor) => (Lists,ProcessorId)= (lists, processor);
        
    }
}