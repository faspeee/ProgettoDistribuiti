    
using Akka.Actor;
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

namespace Zyzzyva
{
    class Program
    {

        static void Main(string[] args)
        {

            var x = StartUp(args.Length == 0 ? new String[] { "2551", "2552", "8080" } : args);
            /*
                        Config config = ConfigurationFactory.Load();
                        ActorSystem system = ActorSystem.Create("cluster-playground");
                        IActorRef node = system.ActorOf(src.Main.Akka.Core.Node.MyProps("localhost"),"node");
                        IActorRef dummy = system.ActorOf(Dummy.MyProps());
            node.Tell(new Node.GetClusterMembers(), dummy);
                        node.Tell(new Node.GetFibonacci(13), dummy);
                        node.Tell(new Node.GetFibonacci(16), dummy);
                        node.Tell(new Node.GetClusterMembers(), dummy);

            */
            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");

            var config =
                   ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.port=" + 1111)
                       .WithFallback(section.AkkaConfig);

            //create an Akka system
            ActorSystem system = ActorSystem.Create("cluster-playground", config);
            //ActorSystem system2 = ActorSystem.Create("cluster-playground");
            IActorRef dummy = system.ActorOf(Dummy.MyProps(),"SONOSCEMO");

            var myRemoteActorSelection = system.ActorSelection("akka.tcp://cluster-playground@localhost:2552/user/node");
            var myRemoteActorSelection1 = system.ActorSelection("akka.tcp://cluster-playground@localhost:2551/user/node");
            Console.ReadLine();
            Console.ReadLine();
            //system2.ActorOf(Dummy.MyProps(), "mememe").Tell(new Node.GetFibonacci(22), dummy);

            myRemoteActorSelection1.Tell(new Node.GetFibonacci(22), dummy);
            myRemoteActorSelection1.Tell(new Node.GetFibonacci(13), dummy);
            myRemoteActorSelection1.Tell(new Node.GetFibonacci(15), dummy);
            myRemoteActorSelection1.Tell(new Node.GetFibonacci(14), dummy);
            myRemoteActorSelection1.Tell(new Node.GetClusterMembers(), dummy);
            myRemoteActorSelection.Tell(new Node.GetFibonacci(14), dummy);
            myRemoteActorSelection1.Tell(new Node.GetFibonacci(15), dummy);
            myRemoteActorSelection.Tell(new Node.GetFibonacci(16), dummy);
            myRemoteActorSelection1.Tell(new Node.GetFibonacci(17), dummy);
            myRemoteActorSelection1.Tell(new Node.GetFibonacci(18), dummy);
            myRemoteActorSelection.Tell(new Node.GetFibonacci(19), dummy);
            myRemoteActorSelection1.Tell(new Node.GetClusterMembers(), dummy);
            myRemoteActorSelection.Tell(new Node.GetFibonacci(44), dummy);
            myRemoteActorSelection1.Tell(new Node.GetFibonacci(44), dummy);
            Console.ReadLine();

        }

        public static List<IActorRef> StartUp(string[] ports)
        {
            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");
            List<IActorRef> list = new();
            foreach (var port in ports)
            {
                //Override the configuration of the port
                var config =
                    ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.port=" + port)
                        .WithFallback(section.AkkaConfig);

                //create an Akka system
                var system = ActorSystem.Create("cluster-playground", config);

                //create an actor that handles cluster domain events
                list.Add(system.ActorOf(Node.MyProps(port), "node"));
            }
            return list;
        }
    }




    class Dummy : ReceiveActor
    {
        public Dummy()
        {
            Receive<ProcessorResponse>(x => Console.WriteLine("COMPUTATO!!!  IL RIS È: " + x.Result + "mandato da " + x.ProcessorId));
            Receive<ListMembers>(x => {
                Console.WriteLine("ARRIVATA RISPOSTA!");
                x.addresses.ForEach(xx => Console.WriteLine("Ci SONO + " + xx));
            }); 
        }

        public static Props MyProps() => Props.Create(() => new Dummy());
    }
}

