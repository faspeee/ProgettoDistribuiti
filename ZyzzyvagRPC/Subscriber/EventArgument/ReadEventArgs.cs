using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zyzzyva.Database.Tables;

namespace ZyzzyvagRPC.Subscriber.EventArgument
{
    public class ReadEventArgs : EventArgs
    {
        public readonly Persona PersonaResult;
        public ReadEventArgs(Persona personas) => PersonaResult = personas;
    }
}
