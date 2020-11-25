using Akka.Actor;
using Akka.Cluster.Tools.Client;
using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Zyzzyva.Akka.Database.Messages;
using Zyzzyva.Akka.Matematica.Messages;
using Zyzzyva.Akka.Membri.Messages;
using Zyzzyva.Database.Tables;

namespace ZyzzyvaRPC.ClusterClientAccess
{
    public sealed class ClusterClientAccess
    {

        private static readonly Lazy<ClusterClientAccess> instance = new (() => new ClusterClientAccess());
        private static readonly ActorSystem system = Create();
        private readonly IActorRef clusterClient;
        //private readonly ClusterClient actorClusterClient;
        private static ActorSystem Create()
        {
            var hocon = File.ReadAllText(ConfigurationManager.AppSettings["configpath"]);
            var section = ConfigurationFactory.ParseString(hocon);
            var config = ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.hostname=" + Dns.GetHostEntry(Dns.GetHostName()).AddressList.First().ToString())
               .WithFallback(section);
            return ActorSystem.Create("cluster",config);
        }

        private ClusterClientAccess()
        {

            

            var t = ImmutableHashSet.Create(ActorPath.Parse("akka.tcp://cluster-playground@zyzzyva:9090/system/receptionist"));
            clusterClient = system.ActorOf(ClusterClient.Props(ClusterClientSettings.Create(system).WithInitialContacts(t)), "client");
            //actorClusterClient = new ClusterClient();

        }

        public static ClusterClientAccess Instance
        {
            get { return instance.Value; }
        }

        internal void Delete(int id, IActorRef actor)
        {
            clusterClient.Tell(new ClusterClient.Send("/user/database_manager", new DeletePersona(id, actor), localAffinity: true));
        }

        internal void Insert(Persona persona, IActorRef actor)
        {
            clusterClient.Tell(new ClusterClient.Send("/user/database_manager", new InsertPersona(persona, actor), localAffinity: true));
        }

        internal void ReadAll(IActorRef actor)
        {
            clusterClient.Tell(new ClusterClient.Send("/user/database_manager", new ReadAllPersona(actor), localAffinity: true));
        }

        internal void Update(Persona persona, IActorRef actor)
        {
            clusterClient.Tell(new ClusterClient.Send("/user/database_manager", new UpdatePersona(persona, actor), localAffinity: true));
        }

        internal void Read(int id, IActorRef actor)
        {
            clusterClient.Tell(new ClusterClient.Send("/user/database_manager", new ReadPersona(id, actor), localAffinity: true));
        }

        public void GetFibonacci(int number, IActorRef sender)
        {
            clusterClient.Tell(new ClusterClient.Send("/user/matematica_manager", new GetFibonacci(number, sender), localAffinity: true));
        }
        public void GetFactorial(int number, IActorRef sender)
        {
            clusterClient.Tell(new ClusterClient.Send("/user/matematica_manager", new GetFactorial(number, sender), localAffinity: true));
        }
        public void GetMembers(IActorRef sender)
        {
            clusterClient.Tell(new ClusterClient.Send("/user/member_manager", new GetClusterMembers(sender), localAffinity: true));
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
