using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using ZyzzyvagRPC.Services;

namespace SMRView.Controller
{
    public class Program
    {
        public (ControllerMatematica, ControllerMember, ControllerPersona) Main()
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new Matematica.MatematicaClient(channel);
                var client2 = new Member.MemberClient(channel);
                var client3 = new DataBase.DataBaseClient(channel);
                return (new ControllerMatematica(client), new ControllerMember(client2), new ControllerPersona(client3));
                
            }
            catch (RpcException e)
            {
                if (e.StatusCode == StatusCode.PermissionDenied)
                {
                    Console.WriteLine("Permission denied.");
                }
            }
            return (null,null,null);
        } 
    }
}
