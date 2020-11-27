using Akka.Actor;

namespace Zyzzyva.Akka.Membri.Messages
{
    /// <include file="../../../Docs/Akka/Membri/Messages/GetMembers.xml" path='docs/members[@name="getmembers"]/GetMembers/*'/>
    public class GetMembers
    {
        /// <include file="../../../Docs/Akka/Membri/Messages/GetMembers.xml" path='docs/members[@name="getmembers"]/ActorRef1/*'/>

        public IActorRef ActorRef1 { get; }

        /// <include file="../../../Docs/Akka/Membri/Messages/GetMembers.xml" path='docs/members[@name="getmembers"]/GetMembersConstructor/*'/>

        public GetMembers(IActorRef actorRef) => ActorRef1 = actorRef;
    };
}
