using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZyzzyvagRPC.ZyzzyvaImplementation.EventArgument;

namespace ZyzzyvagRPC.Subscriber.EventArgument
{
    public static class ExtendsMethod
    {
        public static FibonacciEventArgs ConvertToFibonacci(this EventArgs args) => (FibonacciEventArgs)args;

        public static FactorialEventArgs ConvertToFactorial(this EventArgs args) => (FactorialEventArgs)args;

        public static MemberEventArgs ConvertToMember(this EventArgs args) => (MemberEventArgs)args;

    }
}
