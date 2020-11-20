using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZyzzyvagRPC.Subscriber.EventArgument
{
    public class FactorialEventArgs : EventArgs
    {
        public int FactorialResult { get; }
        public FactorialEventArgs(int factorialResult) => FactorialResult = factorialResult;
    }
}
