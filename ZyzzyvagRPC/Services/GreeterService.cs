using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZyzzyvagRPC
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }
 
        public override Task<GetFibonacciReply> GetFibonacci(GetFibonacciRequest number, ServerCallContext context)
        {
            return Task.FromResult(new GetFibonacciReply
            {
                Number =number.Number+13
            });
        }

        public override async Task<GetMemberReply> GetClusterMembers(GetMemberRequest request, ServerCallContext context)
        {
            var response = new GetMemberReply();
            await Task.Run(() =>
            { 
                response.Members.Add("YO SOY IL BEST");
            });
            return response;
        }
    }
}
