using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Zyzzyva.Database.Tables;

namespace Zyzzyva.Akka.Database.Messages
{
    public class InsertPersonaResponse
    {
        public readonly ImmutableList<Persona> Personas;

        public readonly string ProcessorId;

        public InsertPersonaResponse(ImmutableList<Persona> personas, string processorId) => (Personas, ProcessorId)=(personas, processorId);
    }
}
