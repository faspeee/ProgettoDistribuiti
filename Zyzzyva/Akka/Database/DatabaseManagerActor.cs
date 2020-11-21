using Akka.Actor;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using Zyzzyva.Akka.Database.Children;
using Zyzzyva.Akka.Database.Messages;
using Zyzzyva.Database;

namespace Zyzzyva.Akka.Database
{
    class DatabaseManagerActor : ReceiveActor
    {
        private readonly PersonaCRUDdb _cRUDdb;
        private IActorRef _databaseRouter;
        public DatabaseManagerActor(string id, PersonaCRUDdb cRUDdb)
        {
            _cRUDdb = cRUDdb;
            _databaseRouter = Context.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "databaseRouter");

            Context.ActorOf(PersonaActor.MyProps(id,_cRUDdb), "database");
            
            Receive<ReadPersona>(msg => _databaseRouter.Forward(msg));
            Receive<ReadAllPersona>(msg => _databaseRouter.Forward(msg));

            Receive<UpdatePersona>(msg => _databaseRouter.Forward(msg));
            Receive<InsertPersona>(msg => _databaseRouter.Forward(msg));
            Receive<DeletePersona>(msg => _databaseRouter.Forward(msg));
        }

    public static Props MyProps(string id, PersonaCRUDdb cRUDdb) => Props.Create(() => new DatabaseManagerActor(id,cRUDdb));

    }
}
