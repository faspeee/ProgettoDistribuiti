using System;
using System.Collections.Generic;
using System.Text;

namespace Zyzzyva.Akka.Database.Messages
{
    public class InsertPersonaResponse
    {
        public readonly int Id;

        public readonly string ProcessorId;

        public InsertPersonaResponse(int id, string processorId) => (Id, ProcessorId)=(id, processorId);
    }
}
