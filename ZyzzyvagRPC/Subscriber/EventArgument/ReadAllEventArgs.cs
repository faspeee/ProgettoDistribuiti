using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Zyzzyva.Database.Tables;

namespace ZyzzyvagRPC.Subscriber.EventArgument
{
    public class ReadAllEventArgs : EventArgs
    {
        public readonly ImmutableList<Persona> PersonaResult;
        public ReadAllEventArgs(ImmutableList<Persona> personas) => PersonaResult = personas;
    }
}
