using Akka.Actor;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zyzzyva.src.Main.Akka.Core
{
    public class Node : ReceiveActor
    {

        private readonly IActorRef _processor;
        private readonly IActorRef _processorRouter;
        private readonly IActorRef _clusterManager;

        public Node(string id)
        {
            _processor = Context.ActorOf(Processor.MyProps(id), "processor");
            _processorRouter = Context.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "processorRouter");
            _clusterManager = Context.ActorOf(ClusterManager.MyProps(id), "clusterManager");

            Receive<GetClusterMembers>(msg => _clusterManager.Forward(new ClusterManager.GetMembers(msg.ActorRef1)));
            Receive<GetFibonacci>(value => _processorRouter.Forward(new Processor.ComputeFibonacci(value.number,value.ActorRef1)));
            Receive<GetFactorial>(value => _processorRouter.Forward(new Processor.ComputeFactorial(value.number, value.ActorRef1)));
        }

        public static Props MyProps(string id) => Props.Create(() => new Node(id));

        public class GetClusterMembers 
        {
            public IActorRef ActorRef1 { get; }

            public GetClusterMembers(IActorRef actorRef) => ActorRef1 =  actorRef;
        }

        public class GetFibonacci
        {
            public int number { get; }
            public IActorRef ActorRef1 { get; }

            public GetFibonacci(int n, IActorRef actorRef) => (number, ActorRef1) = (n,actorRef);
        }

        public class GetFactorial
        {
            public int number { get; }
            public IActorRef ActorRef1 { get; }

            public GetFactorial(int n, IActorRef actorRef) => (number, ActorRef1) = (n, actorRef);
        }
    }
}