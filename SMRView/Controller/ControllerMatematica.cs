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
    public class ControllerMatematica : CancellationTokenAbstract, IAsyncDisposable, IControllerMatematica
    {
        private readonly Matematica.MatematicaClient _clientMatematica;
        private readonly AsyncDuplexStreamingCall<MatematicaRequest, MatematicaResponse> _duplexStreamMatematica;
        private readonly Task _responseTaskMatematica;

        public ControllerMatematica(Matematica.MatematicaClient clientMatematica)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _clientMatematica = clientMatematica;
            _duplexStreamMatematica = _clientMatematica.Subscribe();
            _responseTaskMatematica = HandleResponsesAsync(_cancellationTokenSource.Token);
        }

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

        private async Task HandleResponsesAsync(CancellationToken token)
        {
            var stream = _duplexStreamMatematica.ResponseStream;

            await foreach (var update in stream.ReadAllAsync(token))
            {
                Trace.WriteLine(Response(update));
            }
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

        private string Response(MatematicaResponse response) => response.ResponseCase switch
        {
            MatematicaResponse.ResponseOneofCase.Msg => "FIBONACCI" + response.Msg.Number,
            MatematicaResponse.ResponseOneofCase.Msg2 => "Factorial" + string.Join(",", response.Msg2.Number),
            MatematicaResponse.ResponseOneofCase.None => "NIO",
            _ => throw new NotImplementedException()
        };
    }
}
