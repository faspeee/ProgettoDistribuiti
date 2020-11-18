using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZyzzyvagRPC;
namespace SMRView.Controller
{
    class CallgRPC
    {
        public async Task GetFibonacci(int number = 6)
        {
            GetFibonacciReply response;

            using (var channel = GrpcChannel.ForAddress("https://localhost:5001"))
            {
                var client = new Greeter.GreeterClient(channel);

                response = await client.GetFibonacciAsync(new GetFibonacciRequest
                {
                    Number = number
                });
            }
 
                Console.WriteLine($"Holding {response.Number} of Share ID {response.Number}.");
            
        }
    }
}
