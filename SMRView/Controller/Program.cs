using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using ZyzzyvagRPC.Services;

namespace SMRView.Controller
{
    public class Program
    {
        public MainWindowViewModel Main()
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new Matematica.MatematicaClient(channel);
                var client2 = new Member.MemberClient(channel);
                return new MainWindowViewModel(client,client2);
                
            }
            catch (RpcException e)
            {
                if (e.StatusCode == StatusCode.PermissionDenied)
                {
                    Console.WriteLine("Permission denied.");
                }
            }
            return null;
        } 
    }
}
