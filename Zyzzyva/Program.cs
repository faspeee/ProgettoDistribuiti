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
        public static void Main(string[] args)
        {
           
             StartUp(args.Length == 1 ? args[0] : "2553");
        } 
        public static (string,string) GetElement(string port)
        {
            var finalport= Environment.GetEnvironmentVariable("CLUSTER_PORT") ?? port;
            var finalIp = Environment.GetEnvironmentVariable("CLUSTER_IP")==null || Environment.GetEnvironmentVariable("CLUSTER_IP").Length==0? Dns.GetHostEntry(Dns.GetHostName()).AddressList.First().ToString(): Environment.GetEnvironmentVariable("CLUSTER_IP");
            return (finalIp, finalport);
        }
        public static void StartUp(string port)
        {
            var (finalIp, finalPort) = GetElement(port);
            var hocon = File.ReadAllText(ConfigurationManager.AppSettings["configpath"]);
            //Console.WriteLine("PORT" + Environment.GetEnvironmentVariable("CLUSTER_PORT")); 
            //Console.WriteLine("IP" + Environment.GetEnvironmentVariable("CLUSTER_IP"));
            Console.WriteLine(Dns.GetHostEntry("zyzzyva").AddressList.First().ToString());
            //Console.WriteLine("hocon" + hocon);

            //Console.WriteLine("finalIP" + finalIp);

            //Console.WriteLine("finalPort" + finalPort);
            var section = ConfigurationFactory.ParseString(hocon);
            //host.AddressList.First();
            var config = ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.hostname="+finalIp)
                .WithFallback(section);

            //var config3 = ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.public-hostname=" + host.AddressList.First().ToString())
            //     .WithFallback(config);
            // var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka"); 
            //Override the configuration of the port 
           //Console.WriteLine("HOLA1" + finalPort);
            var config2 =
                ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.port=" + finalPort)
                    .WithFallback(config);//.BootstrapFromDocker();

           // Console.WriteLine("HOLA2" + config2.ToString());


           // var tt =Environment.GetEnvironmentVariable("CLUSTER_IP").Length!=0? Environment.GetEnvironmentVariable("CLUSTER_IP") : "127.0.0.1";
           //  "akka.tcp://cluster-playground@127.0.0.1:2551"
           // var config3 =ConfigurationFactory.ParseString("akka.cluster.seed-nodes="+ $"\"[akka.tcp://cluster-playground@{tt}:9090]\"").WithFallback(config2);
            //create an Akka system
           // Console.WriteLine("HOLA3"+config3);
            var system = ActorSystem.Create("cluster-playground", config2);
           
            //create an actor that handles cluster domain events
            var matematicaManager = system.ActorOf(MatematicaManagerActor.MyProps(port), "matematica_manager");
            var membriManager = system.ActorOf(MembriManagerActor.MyProps(port), "member_manager");
            var databaseManager = system.ActorOf(DatabaseManagerActor.MyProps(port), "database_manager");
            ClusterClientReceptionist.Get(system).RegisterService(matematicaManager);
            ClusterClientReceptionist.Get(system).RegisterService(membriManager);
            ClusterClientReceptionist.Get(system).RegisterService(databaseManager);
            system.WhenTerminated.Wait();
        }
    }
}
