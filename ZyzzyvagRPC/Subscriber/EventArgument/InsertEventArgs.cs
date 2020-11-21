using System;
using System.Collections.Immutable;
using Zyzzyva.Database.Tables;

namespace ZyzzyvagRPC.Subscriber.EventArgument
{
    public class InsertEventArgs : EventArgs
    {
        public readonly ImmutableList<Persona> PersonaResult;
        public InsertEventArgs(ImmutableList<Persona> personas) => PersonaResult = personas;
    }
}
