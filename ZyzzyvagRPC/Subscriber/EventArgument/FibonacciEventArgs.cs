using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZyzzyvagRPC.ZyzzyvaImplementation.EventArgument
{
    public class FibonacciEventArgs : EventArgs
    {
        public int FibonacciResult { get; }
        public FibonacciEventArgs(int fibonacciResult)=>FibonacciResult = fibonacciResult;
        
    } 
}
