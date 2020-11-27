using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZyzzyvagRPC.Subscriber;
using ZyzzyvagRPC.ZyzzyvaImplementation.EventArgument;

namespace ZyzzyvagRPC.Subscriber.SubscriberContract
{
    /// <include file="../../Docs/Subscriber/SubscriberContract/IMemberSubscriber.xml" path='docs/members[@name="imembersubscriber"]/IPersonSubscriber/*'/>
    public interface IMemberSubscriber : ISubscriber
    { 
        event EventHandler<MemberEventArgs> MemberEvent;

        /// <include file="../../Docs/Subscriber/SubscriberContract/IMemberSubscriber.xml" path='docs/members[@name="imembersubscriber"]/GetMembers/*'/>
        void GetMembers();  
    }

    

   
}
