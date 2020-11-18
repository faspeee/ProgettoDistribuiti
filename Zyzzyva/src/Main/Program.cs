    
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
    class Program
    {

        IActorRef c, dummy;
        

        async Task Main()
        {

            StartUp(new String[] { "2551", "2552", "8080" });
            
            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");

            var config =
                   ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.port=" + 1111)
                       .WithFallback(section.AkkaConfig);

            //create an Akka system
            ActorSystem system = ActorSystem.Create("cluster");
            //ActorSystem system2 = ActorSystem.Create("cluster-playground");
            dummy = system.ActorOf(Dummy.MyProps(),"SONOSCEMO");

            await Task.Delay(4000);

            var t = ImmutableHashSet.Create(ActorPath.Parse("akka.tcp://cluster-playground@localhost:2552/system/receptionist"));
            c = system.ActorOf(ClusterClient.Props(ClusterClientSettings.Create(system).WithInitialContacts(t)), "client");
            await Task.Delay(4000);

            

            ////system2.ActorOf(Dummy.MyProps(), "mememe").Tell(new Node.GetFibonacci(22), dummy);

            //myRemoteActorSelection1.Tell(new Node.GetFibonacci(22), dummy);
            //myRemoteActorSelection1.Tell(new Node.GetFibonacci(13), dummy);
            //myRemoteActorSelection1.Tell(new Node.GetFibonacci(15), dummy);
            //myRemoteActorSelection1.Tell(new Node.GetFibonacci(14), dummy);
            //myRemoteActorSelection1.Tell(new Node.GetClusterMembers(), dummy);
            //myRemoteActorSelection.Tell(new Node.GetFibonacci(14), dummy);
            //myRemoteActorSelection1.Tell(new Node.GetFibonacci(15), dummy);
            //myRemoteActorSelection.Tell(new Node.GetFibonacci(16), dummy);
            //myRemoteActorSelection1.Tell(new Node.GetFibonacci(17), dummy);
            //myRemoteActorSelection1.Tell(new Node.GetFibonacci(18), dummy);
            //myRemoteActorSelection.Tell(new Node.GetFibonacci(19), dummy);
            //myRemoteActorSelection1.Tell(new Node.GetClusterMembers(), dummy);
            //myRemoteActorSelection.Tell(new Node.GetFibonacci(44), dummy);
            //myRemoteActorSelection1.Tell(new Node.GetFibonacci(44), dummy);
            //Console.ReadLine();

        }

        public async Task<object> sticazzi()
        {
            return await c.Ask(new ClusterClient.Send("/user/node", new Node.GetFibonacci(22,dummy), localAffinity: true));
            //c.Tell(new ClusterClient.Send("/user/node", new Node.GetFibonacci(22, dummy), localAffinity: true));
            //c.Tell(new ClusterClient.Send("/user/node", new Node.GetFibonacci(22, dummy), localAffinity: true));
            //c.Tell(new ClusterClient.Send("/user/node", new Node.GetFibonacci(22, dummy), localAffinity: true));
            //c.Tell(new ClusterClient.Send("/user/node", new Node.GetFibonacci(22, dummy), localAffinity: true));
        }

        public static void StartUp(string[] ports)
        { 

            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");
             foreach (var port in ports)
            {
                //Override the configuration of the port
                var config =
                    ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.port=" + port)
                        .WithFallback(section.AkkaConfig);

                //create an Akka system
                var system = ActorSystem.Create("cluster-playground", config);

                //create an actor that handles cluster domain events
                var node = system.ActorOf(Node.MyProps(port), "node");
                ClusterClientReceptionist.Get(system).RegisterService(node); 
            }


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

