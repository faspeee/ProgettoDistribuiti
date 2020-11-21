using Grpc.Core;
using SMRView.Controller.ControllerContract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZyzzyvagRPC.Services;

namespace SMRView.Controller
{
    public class ControllerMatematica : BaseController, IAsyncDisposable, IControllerMatematica
    {
        private static readonly Lazy<ControllerMatematica> instance = new(() => new ControllerMatematica());

        private static readonly Matematica.MatematicaClient _clientMatematica = new Matematica.MatematicaClient(channel);

        private static readonly AsyncDuplexStreamingCall<MatematicaRequest, MatematicaResponse> _duplexStreamMatematica = _clientMatematica.Subscribe();
        private static readonly Task _responseTaskMatematica = HandleResponsesAsync(_cancellationTokenSource.Token);
                
        private ControllerMatematica()
        {
        }

        public static ControllerMatematica Instance => instance.Value;

        public async Task Fibonacci(int number)
        {
            var x = new MatematicaRequest { Msg = new FibonacciRequest { Number = number } };
            await _duplexStreamMatematica.RequestStream.WriteAsync(x);
        }
        public async Task Factorial(int number)
        {
            var x = new MatematicaRequest { Msg2 = new FactorialRequest { Number = number } };
            await _duplexStreamMatematica.RequestStream.WriteAsync(x);
        }

        public async ValueTask DisposeAsync()
        {
            _cancellationTokenSource.Cancel();
            try
            {
                await _duplexStreamMatematica.RequestStream.CompleteAsync().ConfigureAwait(false);
                await _responseTaskMatematica.ConfigureAwait(false);
            }
            finally
            {
                _duplexStreamMatematica.Dispose();
            }
        }

        private static async Task HandleResponsesAsync(CancellationToken token)
        {
            var stream = _duplexStreamMatematica.ResponseStream;

            await foreach (var update in stream.ReadAllAsync(token))
            {
                Trace.WriteLine(Response(update));
            }
        } 
        
        

        private static string Response(MatematicaResponse response) => response.ResponseCase switch
        {
            MatematicaResponse.ResponseOneofCase.Msg => "FIBONACCI" + response.Msg.Number,
            MatematicaResponse.ResponseOneofCase.Msg2 => "Factorial" + string.Join(",", response.Msg2.Number),
            MatematicaResponse.ResponseOneofCase.None => "NIO",
            _ => throw new NotImplementedException()
        };
    }
}
