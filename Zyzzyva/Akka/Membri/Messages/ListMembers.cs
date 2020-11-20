using System.Collections.Generic;

namespace Zyzzyva.Akka.Membri.Messages
{
    public class ListMembers
    {
        public List<string> addresses { get; }

        public ListMembers(List<string> members) => addresses = members;
    }
}
