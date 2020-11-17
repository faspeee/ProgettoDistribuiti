    
using Akka.Actor;
using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using Zyzzyva.src.Main.Akka.Core;

namespace Zyzzyva
{
    class Program
    {
        static void Main(string[] args)
        {

            Config config = ConfigurationFactory.Load();
            ActorSystem system = ActorSystem.Create("cluster-playground");
            var node = system.ActorOf(src.Main.Akka.Core.Node.MyProps("127.0.0.1"),"node");

            var t = node.Ask(new Node.GetClusterMembers());
            Console.WriteLine("WEEEEEEEEEEEEE "+ string.Join("",((List<string>)t.Result)));
            Console.ReadLine(); 
        }
    }
}

