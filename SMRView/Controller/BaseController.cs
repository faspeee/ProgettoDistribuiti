using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;

namespace SMRView.Controller
{
    public abstract class BaseController
    {
        protected static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        protected static GrpcChannel channel = GrpcChannel.ForAddress(ConfigurationManager.ConnectionStrings["serverPath"].ConnectionString);
    }
}
