using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Grpc.Core;
using ZyzzyvagRPC.Checazzonesoio;
using ZyzzyvagRPC.Subscriber.SubscriberContract;

namespace ZyzzyvagRPC.Services
{
 
    public class MatematicaService : Matematica.MatematicaBase
    {
        private readonly ISubscriberFactory _factoryMethod;
        private readonly ILogger<MatematicaService> _logger;
        public MatematicaService(ISubscriberFactory factoryMethod, ILogger<MatematicaService> logger)
        {
            _logger = logger;
            _factoryMethod = factoryMethod;
        }

        public override async Task Subscribe(IAsyncStreamReader<MatematicaRequest> request, IServerStreamWriter<MatematicaResponse> response, ServerCallContext context)
        {
            using var subscriberF = _factoryMethod.GetFibonacciSubscriber(); 

            subscriberF.FibonacciEvent += async (sender, args) =>
                await WriteUpdateAsyncFib(response, args.FibonacciResult);

            subscriberF.FactorialEvent += async (sender, args) =>
            await WriteUpdateAsyncFac(response, args.FactorialResult);

            subscriberF.CreateActor(); 
            var actionsTask = HandleActions(request, subscriberF, context.CancellationToken); 

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

        private async Task WriteUpdateAsyncFib(IServerStreamWriter<MatematicaResponse> stream, int fibonacci)
        {

            //var reply = new GetMemberReply();
            try
            {
                await stream.WriteAsync(new MatematicaResponse
                {
                    Msg = new FibonacciReply
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

        private async Task WriteUpdateAsyncFac(IServerStreamWriter<MatematicaResponse> stream, int factorial)
        {

            //var reply = new GetMemberReply();
            try
            {
                await stream.WriteAsync(new MatematicaResponse
                {
                    Msg2 = new FactorialReply
                    {
                        Number = factorial
                    }

                });
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to write message: {e.Message}");
            }
        }

        private async Task HandleActions(IAsyncStreamReader<MatematicaRequest> requestStream, IFibonacciSubscriber subscriber, CancellationToken token)
        {
            await foreach (var action in requestStream.ReadAllAsync(token))
            {
                switch (action.ActionCase)
                {
                    case MatematicaRequest.ActionOneofCase.None:
                        _logger.LogWarning("No Action specified.");
                        break;
                    case MatematicaRequest.ActionOneofCase.Msg:
                        subscriber.GetFibonacci(action.Msg.Number);
                        break;
                    case MatematicaRequest.ActionOneofCase.Msg2:
                        subscriber.GetFactorial(action.Msg2.Number);
                        break;
                    default:
                        _logger.LogWarning($"Unknown Action '{action.ActionCase}'.");
                        break;
                }
            }
        }
        
    }
}
