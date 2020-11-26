using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zyzzyva.Database.Tables;
using ZyzzyvagRPC.Subscriber.EventArgument;

namespace ZyzzyvagRPC.Subscriber.SubscriberContract
{
    /// <include file="../../Docs/Subscriber/SubscriberContract/IPersonSubscriber.xml" path='docs/members[@name="ipersonasubscriber"]/IPersonSubscriber/*'/>
    public interface IPersonSubscriber : ISubscriber
    {
        event EventHandler<ReadEventArgs> ReadEvent;
        event EventHandler<ReadAllEventArgs> ReadAllEvent;
        event EventHandler<InsertEventArgs> InsertEvent;
        event EventHandler<UpdateEventArgs> UpdateEvent;
        event EventHandler<DeleteEventArgs> DeleteEvent;

        /// <include file="../../Docs/Subscriber/SubscriberContract/IPersonSubscriber.xml" path='docs/members[@name="isubscriber"]/Read/*'/>
        void Read(int id);
        /// <include file="../../Docs/Subscriber/SubscriberContract/IPersonSubscriber.xml" path='docs/members[@name="isubscriber"]/ReadAll/*'/>
        void ReadAll();
        /// <include file="../../Docs/Subscriber/SubscriberContract/IPersonSubscriber.xml" path='docs/members[@name="isubscriber"]/Insert/*'/>
        void Insert(Persona persona);
        /// <include file="../../Docs/Subscriber/SubscriberContract/IPersonSubscriber.xml" path='docs/members[@name="isubscriber"]/Update/*'/>
        void Update(Persona persona);
        /// <include file="../../Docs/Subscriber/SubscriberContract/IPersonSubscriber.xml" path='docs/members[@name="isubscriber"]/Delete/*'/>
        void Delete(int id);
    }
}
