using System;
using System.Collections.Generic;
using System.Text;
using Zyzzyva.Database.Tables;

namespace Zyzzyva.Akka.Database.Messages
{
    public class ReadPersonaResponse
    {
        public readonly Persona Persona;

        public readonly string ProcessorId;

        public ReadPersonaResponse(Persona persona, string id) => (Persona, ProcessorId) = (persona,id);
    }
}
