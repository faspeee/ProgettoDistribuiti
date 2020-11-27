using Akka.Actor;

namespace Zyzzyva.Akka.Membri.Messages
{
    /// <include file="../../../Docs/Akka/Membri/Messages/GetClusterMember.xml" path='docs/members[@name="getclustermember"]/GetClusterMember/*'/>

    public class GetClusterMembers
    {
        /// <include file="../../../Docs/Akka/Membri/Messages/GetClusterMember.xml" path='docs/members[@name="getclustermember"]/ActorRef1/*'/>

        public IActorRef ActorRef1 { get; }
        /// <include file="../../../Docs/Akka/Membri/Messages/GetClusterMember.xml" path='docs/members[@name="getclustermember"]/GetClusterMembersConstructor/*'/>

        public GetClusterMembers(IActorRef actorRef) => ActorRef1 = actorRef;
    }
}
