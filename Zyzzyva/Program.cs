using System;
using Akka.Actor;
using Akka.Cluster.Tools.Client;
using Akka.Configuration;
using Akka.Configuration.Hocon;
using System.Configuration;
using System.Threading.Tasks;
using Zyzzyva.Database.Tables;
using Zyzzyva.Akka.Matematica;
using Hocon;
using System.IO;
using Zyzzyva.Akka.Membri;
using Zyzzyva.Akka.Database;
using Zyzzyva.Database;

namespace Zyzzyva
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Task.Run(() => StartUp(args.Length == 1 ? args[0] : "2554"));
        }

        public static void StartUp(string port)
        {

            var hocon = File.ReadAllText("~/../../Zyzzyva/ActorHocon.hocon");
            var section = ConfigurationFactory.ParseString(hocon);
            // var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");

            //Override the configuration of the port
            var config =
                ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.port=" + port)
                    .WithFallback(section);

            //create an Akka system
            var system = ActorSystem.Create("cluster-playground", config);

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

