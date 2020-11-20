using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Grpc.Core;
using ZyzzyvagRPC.Checazzonesoio;
using System.Collections.Generic;
using ZyzzyvagRPC.Subscriber.SubscriberContract;

namespace ZyzzyvagRPC.Services
{
    public class MemberService : Member.MemberBase
    {
        private readonly ISubscriberFactory _factoryMethod;
        private readonly ILogger<MemberService> _logger;
        public MemberService(ISubscriberFactory factoryMethod, ILogger<MemberService> logger)
        {
            _logger = logger;
            _factoryMethod = factoryMethod;
        }

        public override async Task Subscribe(IAsyncStreamReader<GetMemberRequest> request, IServerStreamWriter<GetMemberReply> response, ServerCallContext context)
        { 
            using var subscriberM = _factoryMethod.GetMemberSubscriber(); 

            subscriberM.MemberEvent += async (sender, args) =>
                await WriteUpdateAsync(response, args.MembersResult);
             
            subscriberM.CreateActor();

            var actionsTask = HandleActions(request, subscriberM, context.CancellationToken); 

            _logger.LogInformation("Subscription started.");
            await AwaitCancellation(context.CancellationToken);

            try
            {
                await actionsTask;
            }
            catch
            {
                /* Ignored */
            }

            _logger.LogInformation("Subscription finished.");
        }

        private static Task AwaitCancellation(CancellationToken token)
        {
            var completion = new TaskCompletionSource<object>();
            token.Register(() => completion.SetResult(null));
            return completion.Task;
        } 

        private async Task WriteUpdateAsync(IServerStreamWriter<GetMemberReply> stream, List<string> members)
        {
             
            try
            {
                var response = new GetMemberReply();
                response.Members.AddRange(members);
                await stream.WriteAsync(response);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to write message: {e.Message}");
            }
        }
         
        private async Task HandleActions(IAsyncStreamReader<GetMemberRequest> requestStream, IMemberSubscriber subscriber, CancellationToken token)
        {
            await foreach (var _ in requestStream.ReadAllAsync(token))
            {
                 subscriber.GetMembers(); 
            }
        }
       
    }
}
