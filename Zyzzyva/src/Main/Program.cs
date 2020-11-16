    
using Akka.Actor;
using Akka.Configuration;
using System;
using System.IO;

namespace Zyzzyva
{
    class Program
    {
        static void Main(string[] args)
        {
            ActorSystem system = ActorSystem.Create("TEST");
            system.ActorOf(src.Main.Akka.Core.Node());
        }
    }
}
