﻿    
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

            Config config = ConfigurationFactory.Load();
            ActorSystem system = ActorSystem.Create("cluster-playground");
            system.ActorOf(src.Main.Akka.Core.Node.MyProps("127.0.0.1"));

            while (true) { };
        }
    }
}

