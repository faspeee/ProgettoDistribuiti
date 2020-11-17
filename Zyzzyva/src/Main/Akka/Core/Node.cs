using Akka.Actor;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zyzzyva.src.Main.Akka.Core
{
    class Node : ReceiveActor
    {

        private readonly IActorRef _processor;
        private readonly IActorRef _processorRouter;
        private readonly IActorRef _clusterManager;

        public Node(string id)
        {
            _processor = Context.ActorOf(Processor.MyProps(id), "processor");
            _processorRouter = Context.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "processorRouter");
            _clusterManager = Context.ActorOf(ClusterManager.MyProps(id), "clusterManager");

            Receive<GetClusterMembers>(_ => _clusterManager.Forward(new ClusterManager.GetMembers()));
            Receive<GetFibonacci>(value => _processorRouter.Forward(new Processor.ComputeFibonacci(value.number)));
        }

        public static Props MyProps(string id) => Props.Create(() => new Node(id));

        public class GetClusterMembers { }

        public class GetFibonacci
        {
            public int number { get; }

            public GetFibonacci(int n) => number = n;
        }
    }
}