using System.Collections.Generic;
using System;
namespace Zyzzyva.Akka.Membri.Messages
{
    /// <include file="../../../Docs/Akka/Membri/Messages/ListMembers.xml" path='docs/members[@name="listmembers"]/ListMembers/*'/>

    public class ListMembers
    {
        /// <include file="../../../Docs/Akka/Membri/Messages/ListMembers.xml" path='docs/members[@name="listmembers"]/Addresses/*'/>

        public List<string> addresses { get; }

        /// <include file="../../../Docs/Akka/Membri/Messages/ListMembers.xml" path='docs/members[@name="listmembers"]/ListMembersConstructor/*'/>
        public ListMembers(List<string> members) => addresses = members;
    }
}
