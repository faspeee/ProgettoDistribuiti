using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using ZyzzyvagRPC.Services;

namespace SMRView.Controller
{
    public class MainWindowViewModel : IAsyncDisposable
    {
        private readonly Matematica.MatematicaClient _client;
        private readonly Member.MemberClient _client2;
        private readonly AsyncDuplexStreamingCall<MatematicaRequest, MatematicaResponse> _duplexStream;
        private readonly AsyncDuplexStreamingCall<GetMemberRequest, GetMemberReply> _duplexStream2;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Task _responseTask;
        private readonly Task _responseTask2;
        private string _addSymbol;

        public MainWindowViewModel(Matematica.MatematicaClient client, Member.MemberClient client2)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _client = client;
            _client2 = client2;
            _duplexStream = _client.Subscribe();
            _duplexStream2 = _client2.Subscribe();
            _responseTask = HandleResponsesAsync(_cancellationTokenSource.Token);
            _responseTask2 = HandleResponsesAsync2(_cancellationTokenSource.Token);

        }

        public async Task Add(int number)
        {
            var x = new MatematicaRequest { Msg = new FibonacciRequest { Number = number } };
               await _duplexStream.RequestStream.WriteAsync(x);
        }
        public async Task Factorial(int number)
        {
            var x = new MatematicaRequest { Msg2= new FactorialRequest { Number = number } };
            await _duplexStream.RequestStream.WriteAsync(x);
        }

        public async Task Members()
        {
            var x = new GetMemberRequest { };
            await _duplexStream2.RequestStream.WriteAsync(x);
        }

        private async Task HandleResponsesAsync(CancellationToken token)
        {
            var stream = _duplexStream.ResponseStream;

            await foreach (var update in stream.ReadAllAsync(token))
            {
                Trace.WriteLine(Response(update));
            }
        }
        private async Task HandleResponsesAsync2(CancellationToken token)
        {
            var stream = _duplexStream2.ResponseStream;

            await foreach (var update in stream.ReadAllAsync(token))
            {
                Trace.WriteLine(string.Join(",", update.Members.ToString()));
            }
        }
        public async ValueTask DisposeAsync()
        {
            _cancellationTokenSource.Cancel();
            try
            {
                await _duplexStream.RequestStream.CompleteAsync().ConfigureAwait(false);
                await _responseTask.ConfigureAwait(false);
                await _duplexStream2.RequestStream.CompleteAsync().ConfigureAwait(false);
                await _responseTask2.ConfigureAwait(false);
            }
            finally
            {
                _duplexStream.Dispose();
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