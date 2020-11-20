using Akka.Actor;
using Akka.Cluster.Tools.Client;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Zyzzyva.src.Main.Akka.Core;

namespace ZyzzyvaRPC.ClusterClientAccess
{
    public sealed class ClusterClientAccess
    {

        private static readonly Lazy<ClusterClientAccess> instance = new (() => new ClusterClientAccess());
        private static readonly ActorSystem system = ActorSystem.Create("cluster");
        private readonly IActorRef clusterClient;
        //private readonly ClusterClient actorClusterClient;


        private ClusterClientAccess()
        {
            var t = ImmutableHashSet.Create(ActorPath.Parse("akka.tcp://cluster-playground@localhost:2552/system/receptionist"));
            clusterClient = system.ActorOf(ClusterClient.Props(ClusterClientSettings.Create(system).WithInitialContacts(t)), "client");
            //actorClusterClient = new ClusterClient();

        }

        public static ClusterClientAccess Instance
        {
            get { return instance.Value; }
        }

        public void GetFibonacci(int number, IActorRef sender)
        {
            clusterClient.Tell(new ClusterClient.Send("/user/node", new Node.GetFibonacci(number, sender), localAffinity: true));
        }
        public void GetFactorial(int number, IActorRef sender)
        {
            clusterClient.Tell(new ClusterClient.Send("/user/node", new Node.GetFactorial(number, sender), localAffinity: true));
        }
        public void GetMembers(IActorRef sender)
        {
            clusterClient.Tell(new ClusterClient.Send("/user/node", new Node.GetClusterMembers(sender), localAffinity: true));
        }

        public static IActorRef CreateActor(Props props)
        {
            return system.ActorOf(props);
        }

        public static void KillActor(IActorRef actor)
        {
            system.Stop(actor);
        }
        
    }
}
