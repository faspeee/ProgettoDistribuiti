using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zyzzyva.Akka.Database.Children
{
    class ServiceScopeExtensionIdProvider : ExtensionIdProvider<ServiceScopeExtension>
    {
        public override ServiceScopeExtension CreateExtension(ExtendedActorSystem system)
        {
            return new ServiceScopeExtension();
        }

        public static ServiceScopeExtensionIdProvider Instance = new ServiceScopeExtensionIdProvider();


    }
}
