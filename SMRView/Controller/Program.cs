using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using ZyzzyvagRPC;

namespace SMRView.Controller
{
    public class Program
    {
        public MainWindowViewModel Main()
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new Greeter.GreeterClient(channel);
                return new MainWindowViewModel(client);
                
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

        public async static Task GetFibonacci(int number = 6)
        {
            await Task.Run(async () =>
            {
                GetFibonacciReply response;

                try
                {
                    using (var channel = GrpcChannel.ForAddress("https://localhost:5001"))
                    {
                        var client = new Greeter.GreeterClient(channel);

                        response = await client.GetFibonacciAsync(new GetFibonacciRequest
                        {
                            Number = number
                        });

                        Console.WriteLine($"Holding {response.Number} of Share ID {response.Number}.");
                    }
                }
                catch (Exception e)
                {
                    e.StackTrace.ToString();
                }
            });


        }


    }
}
