using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZyzzyvagRPC.Subscriber;
using ZyzzyvagRPC.ZyzzyvaImplementation.EventArgument;

namespace ZyzzyvagRPC.Subscriber.SubscriberContract
{
    public interface IMemberSubscriber : ISubscriber
    { 
        event EventHandler<MemberEventArgs> MemberEvent; 
        void GetMembers();  
    }

    

   
}
