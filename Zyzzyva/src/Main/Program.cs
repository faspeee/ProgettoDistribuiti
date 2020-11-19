
using Akka.Actor;
using Akka.Cluster.Tools.Client;
using Akka.Configuration;
using Akka.Configuration.Hocon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using Zyzzyva.src.Main.Akka.Core;
using static Zyzzyva.src.Main.Akka.Core.ClusterManager;
using static Zyzzyva.src.Main.Akka.Core.ProcessorFibonacci;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Zyzzyva
{
    public class Program
    {
        public static void Main(string[] args)
        {

            new src.Main.Database.CRUDdb("").readOrdini().ForEach(x => Console.WriteLine(x.customer+x.quant));
            Console.ReadLine();
            //Task.Run(() => StartUp(args.Length == 1 ? args[0] : "2551"));
        }

        public static void StartUp(string port)
        {
            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");

            //Override the configuration of the port
            var config =
                ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.port=" + port)
                    .WithFallback(section.AkkaConfig);

            //create an Akka system
            var system = ActorSystem.Create("cluster-playground", config);

            //create an actor that handles cluster domain events
            var node = system.ActorOf(Node.MyProps(port), "node");
            ClusterClientReceptionist.Get(system).RegisterService(node);
            system.WhenTerminated.Wait();
        }
    }
}

