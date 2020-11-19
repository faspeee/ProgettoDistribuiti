using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using ZyzzyvagRPC;

namespace SMRView.Controller
{
    public class MainWindowViewModel : IAsyncDisposable
    {
        private readonly Greeter.GreeterClient _client;
        private readonly AsyncDuplexStreamingCall<ServerRequest, ServerResponse> _duplexStream;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Task _responseTask;
        private string _addSymbol;

        public MainWindowViewModel(Greeter.GreeterClient client)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _client = client;
            _duplexStream = _client.Subscribe();
            _responseTask = HandleResponsesAsync(_cancellationTokenSource.Token);

        }

        public async Task Add(int number)
        {
            var x = new ServerRequest { Msg = new GetFibonacciRequest { Number = number } };
               await _duplexStream.RequestStream.WriteAsync(x);
        }

        public async Task Members()
        {
            var x = new ServerRequest { Msg2 = new GetMemberRequest { } };
            await _duplexStream.RequestStream.WriteAsync(x);
        }

        private async Task HandleResponsesAsync(CancellationToken token)
        {
            var stream = _duplexStream.ResponseStream;

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
                await _duplexStream.RequestStream.CompleteAsync().ConfigureAwait(false);
                await _responseTask.ConfigureAwait(false);
            }
            finally
            {
                _duplexStream.Dispose();
            }
        }

        private string Response(ServerResponse response) => response.ACaso2Case switch
        {
            ServerResponse.ACaso2OneofCase.Msg => "FIBONACCI" + response.Msg.Number ,
            ServerResponse.ACaso2OneofCase.Msg2 => "MEMBRI" + string.Join(",", response.Msg2.Members),
            ServerResponse.ACaso2OneofCase.None => "NIO"
        };
    }
}