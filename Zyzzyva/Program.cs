using System;
using Akka.Actor;
using Akka.Cluster.Tools.Client;
using Akka.Configuration;
using System.Configuration;
using Zyzzyva.Akka.Matematica;
using System.IO;
using Zyzzyva.Akka.Membri;
using Zyzzyva.Akka.Database; 
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace Zyzzyva
{
    public class Program
    {

        private static readonly string MATEMATICA_NAME = "matematica_manager";
        private static readonly string DATABASE_NAME = "database_manager";
        private static readonly string MEMBER_NAME = "member_manager";

        public static void Main(string[] args)
        {
             StartUp(args.Length == 1 ? args[0] : "0");
        } 
        
        private static void StartUp(string port)
        {
            var (finalIp, finalPort) = GetElement(port);
            var hocon = File.ReadAllText(ConfigurationManager.AppSettings["configpath"]);
            var section = ConfigurationFactory.ParseString(hocon);
            var config = ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.hostname="+finalIp)
                .WithFallback(section);

            var config2 =
                ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.port=" + finalPort)
                    .WithFallback(config);

            var system = ActorSystem.Create("cluster-playground", config2);

            var matematicaManager = system.ActorOf(MatematicaManagerActor.MyProps(MATEMATICA_NAME), MATEMATICA_NAME);
            var membriManager = system.ActorOf(MembriManagerActor.MyProps(MEMBER_NAME), MEMBER_NAME);
            var databaseManager = system.ActorOf(DatabaseManagerActor.MyProps(DATABASE_NAME), DATABASE_NAME);
            ClusterClientReceptionist.Get(system).RegisterService(matematicaManager);
            ClusterClientReceptionist.Get(system).RegisterService(membriManager);
            ClusterClientReceptionist.Get(system).RegisterService(databaseManager);
            system.WhenTerminated.Wait();
        }

        private static (string, string) GetElement(string port)
        {
            var finalport = Environment.GetEnvironmentVariable("CLUSTER_PORT") ?? port;
            var finalIp = Environment.GetEnvironmentVariable("CLUSTER_IP") == null || Environment.GetEnvironmentVariable("CLUSTER_IP").Length == 0 ? Dns.GetHostEntry(Dns.GetHostName()).AddressList.First().ToString() : Environment.GetEnvironmentVariable("CLUSTER_IP");
            return (finalIp, finalport);
        }
    }
}
