using Akka.Actor;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZyzzyvaRPC.ClusterClientAccess;
using Zyzzyva.src.Main.Akka;
using static Zyzzyva.src.Main.Akka.Core.ProcessorFibonacci;
using static Zyzzyva.src.Main.Akka.Core.ClusterManager;
using ZyzzyvagRPC.Checazzonesoio;
using System.Threading;

namespace ZyzzyvagRPC
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly IMethodSubscriberFactory _factoryMethod;
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(IMethodSubscriberFactory factoryMethod,ILogger<GreeterService> logger)
        {
            _logger = logger;
            _factoryMethod = factoryMethod;
        }

        public override async Task Subscribe(IAsyncStreamReader<ServerRequest> request, IServerStreamWriter<ServerResponse> response,ServerCallContext context)
        {
            using var subscriber = _factoryMethod.GetSubscriber();

            subscriber.Update += async (sender, args) =>
                await WriteUpdateAsync(response, args.Boh);

            subscriber.UpdateMembers += async (sender, args) =>
                await WriteUpdateAsync(response, args.Boh);

            subscriber.CreateActor();

            var actionsTask = HandleActions(request, subscriber, context.CancellationToken);

            _logger.LogInformation("Subscription started.");
            await AwaitCancellation(context.CancellationToken);

            try { await actionsTask; } catch { /* Ignored */ }

            _logger.LogInformation("Subscription finished.");
        }

        private static Task AwaitCancellation(CancellationToken token)
        {
            var completion = new TaskCompletionSource<object>();
            token.Register(() => completion.SetResult(null));
            return completion.Task;
        }

        private async Task WriteUpdateAsync(IServerStreamWriter<ServerResponse> stream, int fibonacci)
        {

            //var reply = new GetMemberReply();
            try
            {
                await stream.WriteAsync(new ServerResponse
                {
                    Msg = new GetFibonacciReply
                    {
                        Number = fibonacci
                    }

                });
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to write message: {e.Message}");
            }
        }

        private async Task WriteUpdateAsync(IServerStreamWriter<ServerResponse> stream, List<string> members)
        {

            var reply = new GetMemberReply();
            reply.Members.AddRange(members);
            try
            {
                await stream.WriteAsync(new ServerResponse
                {
                    Msg2 = reply

                });
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to write message: {e.Message}");
            }
        }

        private async Task HandleActions(IAsyncStreamReader<ServerRequest> requestStream, IMethodSubscriber subscriber, CancellationToken token)
        {
            await foreach (var action in requestStream.ReadAllAsync(token))
            {
                switch (action.ACasoCase)
                {
                    case ServerRequest.ACasoOneofCase.None:
                        _logger.LogWarning("No Action specified.");
                        break;
                    case ServerRequest.ACasoOneofCase.Msg:
                        subscriber.GetFibonacci(action.Msg.Number);
                        break;
                    case ServerRequest.ACasoOneofCase.Msg2:
                        subscriber.GetMembers();
                        break;
                    default:
                        _logger.LogWarning($"Unknown Action '{action.ACasoCase}'.");
                        break;
                }
            }
        }
        public override Task<GetFibonacciReply> GetFibonacci(GetFibonacciRequest number, ServerCallContext context)
        {

            return Task.FromResult(new GetFibonacciReply
            {
                Number = number.Number+13
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
