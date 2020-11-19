using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZyzzyvagRPC.Checazzonesoio
{
    public interface IMethodSubscriber : IDisposable
    {
        event EventHandler<MethodUpdateEventArgs> Update;
        event EventHandler<MethodUpdateEventArgs2> UpdateMembers;
        void GetFibonacci(int number);
        void GetMembers();

        void CreateActor();
    }

    public class MethodUpdateEventArgs : EventArgs
    {
        public int Boh { get; }
        public MethodUpdateEventArgs(int bho)
        {
            Boh = bho;
        }
    }

    public class MethodUpdateEventArgs2 : EventArgs
    {
        public List<string> Boh { get; }
        public MethodUpdateEventArgs2(List<string> bho)
        {
            Boh = bho;
        }
    }
}
