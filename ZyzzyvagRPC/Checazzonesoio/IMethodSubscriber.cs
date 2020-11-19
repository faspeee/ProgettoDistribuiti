using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZyzzyvagRPC.Checazzonesoio
{
    public interface IMethodSubscriber : IDisposable
    {
        event EventHandler<MethodUpdateEventArgs> Update;
        void GetFibonacci(int number);
        void GetMembers();

    }

    public class MethodUpdateEventArgs : EventArgs
    {
        public int Boh { get; }
        public MethodUpdateEventArgs(int bho)
        {
            Boh = bho;
        }
    }
}
