using System;
using System.Collections.Generic;

namespace ZyzzyvagRPC.ZyzzyvaImplementation.EventArgument
{
    public class MemberEventArgs : EventArgs
    {

        public List<string> MembersResult { get; }
        public MemberEventArgs(List<string> members) => MembersResult = members;
    }
}
  