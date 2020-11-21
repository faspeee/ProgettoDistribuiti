using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zyzzyva.Database.Tables;
using ZyzzyvagRPC.Subscriber.EventArgument;

namespace ZyzzyvagRPC.Subscriber.SubscriberContract
{
    public interface IPersonSubscriber : ISubscriber
    {
        event EventHandler<ReadEventArgs> ReadEvent;
        event EventHandler<ReadAllEventArgs> ReadAllEvent;
        event EventHandler<InsertEventArgs> InsertEvent;
        event EventHandler<UpdateEventArgs> UpdateEvent;
        event EventHandler<DeleteEventArgs> DeleteEvent;
        void Read(int id);
        void ReadAll();
        void Insert(Persona persona);
        void Update(Persona persona);
        void Delete(int id);
    }
}
