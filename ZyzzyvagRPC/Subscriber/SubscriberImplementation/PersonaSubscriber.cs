using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zyzzyva.Akka.Database.Children;
using Zyzzyva.Akka.Database.Messages;
using Zyzzyva.Database.Tables;
using ZyzzyvagRPC.Subscriber.EventArgument;
using ZyzzyvagRPC.Subscriber.SubscriberContract;
using ZyzzyvaRPC.ClusterClientAccess;

namespace ZyzzyvagRPC.Subscriber.SubscriberImplementation
{

    /// <include file="../../Docs/Subscriber/SubscriberImplementation/PersonaSubscriber.xml" path='docs/members[@name="personasubscriber"]/PersonaSubscriber/*'/> 
    public class PersonaSubscriber : AbstractSubscriber, IPersonSubscriber
    {
        public event EventHandler<ReadEventArgs> ReadEvent;
        public event EventHandler<ReadAllEventArgs> ReadAllEvent;
        public event EventHandler<InsertEventArgs> InsertEvent;
        public event EventHandler<UpdateEventArgs> UpdateEvent;
        public event EventHandler<DeleteEventArgs> DeleteEvent;


        /// <inheritdoc/>
        public override void CreateActor()=> _actor = ClusterClientAccess.CreateActor(PersonaActor.MyProps(this, ReadEvent, ReadAllEvent, InsertEvent, UpdateEvent, DeleteEvent));

        /// <inheritdoc/>
        public void Delete(int id) => ClusterClientAccess.Instance.Delete(id, _actor);

        /// <inheritdoc/>
        public void Insert(Persona persona) => ClusterClientAccess.Instance.Insert(persona, _actor);

        /// <inheritdoc/>
        public void Read(int id) => ClusterClientAccess.Instance.Read(id, _actor);

        /// <inheritdoc/>
        public void ReadAll() => ClusterClientAccess.Instance.ReadAll(_actor);

        /// <inheritdoc/>
        public void Update(Persona persona) => ClusterClientAccess.Instance.Update(persona, _actor); 
                                                                                                         
        private class PersonaActor : ReceiveActor
        {
            public event EventHandler<ReadEventArgs>   ReadEvent;
            public event EventHandler<ReadAllEventArgs>ReadAllEvent;
            public event EventHandler<InsertEventArgs> InsertEvent;
            public event EventHandler<UpdateEventArgs> UpdateEvent;
            public event EventHandler<DeleteEventArgs> DeleteEvent; 
            private readonly PersonaSubscriber _personaSubscriber;
            //private Stream stream;
            public PersonaActor(PersonaSubscriber persona, EventHandler<ReadEventArgs> readEvent, EventHandler<ReadAllEventArgs> readAllEvent, EventHandler<InsertEventArgs> insertEvent,
                EventHandler<UpdateEventArgs> updateEvent, EventHandler<DeleteEventArgs> deleteEvent)
            {
                (ReadEvent, ReadAllEvent, InsertEvent, UpdateEvent, DeleteEvent) = (readEvent, readAllEvent, insertEvent, updateEvent, deleteEvent);
                _personaSubscriber = persona; 
                Receive<InsertPersonaResponse>(x =>  InsertEvent?.Invoke(_personaSubscriber, new InsertEventArgs(x.Personas)));
                Receive<UpdatePersonaResponse>(x =>  UpdateEvent?.Invoke(_personaSubscriber, new UpdateEventArgs(x.Persona)));
                Receive<DeletePersonaResponse>(x =>  DeleteEvent?.Invoke(_personaSubscriber, new DeleteEventArgs(x.Personas)));
                Receive<ReadPersonaResponse>(x =>    ReadEvent?.Invoke(_personaSubscriber, new ReadEventArgs(x.Persona)));
                Receive<ReadAllPersonaResponse>(x => ReadAllEvent?.Invoke(_personaSubscriber, new ReadAllEventArgs(x.Lists)));
            }

            public static Props MyProps(PersonaSubscriber persona, EventHandler<ReadEventArgs> readEvent, EventHandler<ReadAllEventArgs> readAllEvent, EventHandler<InsertEventArgs> insertEvent,
                EventHandler<UpdateEventArgs> updateEvent, EventHandler<DeleteEventArgs> deleteEvent) => Props.Create(() => new PersonaActor(persona, readEvent, readAllEvent, insertEvent, updateEvent, deleteEvent));
        }
    }
}
