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
    public class ControllerPersona : BaseController, IAsyncDisposable, IControllerPersona
    {

        private static readonly Lazy<ControllerPersona> instance = new(() => new ControllerPersona());

        private static readonly DataBase.DataBaseClient _clientPersona = new DataBase.DataBaseClient(channel);

        private static readonly AsyncDuplexStreamingCall<ReadRequest, ReadResponseS> _duplexStreamPersonaRead = _clientPersona.SubscribeRead();
        private static readonly AsyncDuplexStreamingCall<WriteRequest, WriteResponse> _duplexStreamPersonaWrite = _clientPersona.SubscribeWrite();
        private static readonly Task _responseTaskPersonaRead = HandleResponsesReadAsync(_cancellationTokenSource.Token);
        private static readonly Task _responseTaskPersonaWrite  = HandleResponsesWriteAsync(_cancellationTokenSource.Token);
        private ControllerPersona()
        {
        }

        public static ControllerPersona Instance => instance.Value;
        
        public async Task Read(int id)
        {
            throw new NotImplementedException();
        }

        public async Task ReadAll()
        {
            var x = new ReadRequest { Msg2 = new ReadAll {} };
            await _duplexStreamPersonaRead.RequestStream.WriteAsync(x);
        }

        public async Task Insert(PersonagRPC persona)
        {
            var x = new WriteRequest { Msg = new Insert { Persona = persona } };
            await _duplexStreamPersonaWrite.RequestStream.WriteAsync(x);
        }

        public async Task Update(PersonagRPC persona)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        } 

        public async ValueTask DisposeAsync()
        {
            _cancellationTokenSource.Cancel();
            try
            {
                await _duplexStreamPersonaWrite.RequestStream.CompleteAsync().ConfigureAwait(false); 
                await _responseTaskPersonaWrite.ConfigureAwait(false);
                await _duplexStreamPersonaRead.RequestStream.CompleteAsync().ConfigureAwait(false);
                await _responseTaskPersonaRead.ConfigureAwait(false);
            }
            finally
            {
                _duplexStreamPersonaWrite.Dispose();
                _duplexStreamPersonaRead.Dispose();
            }
        }

        private static async Task HandleResponsesReadAsync(CancellationToken token)
        {
            var stream = _duplexStreamPersonaRead.ResponseStream;

            await foreach (var update in stream.ReadAllAsync(token))
            {
                Trace.WriteLine(ResponseRead(update));
            }
        }
        private static async Task HandleResponsesWriteAsync(CancellationToken token)
        {
            var stream = _duplexStreamPersonaWrite.ResponseStream;

            await foreach (var update in stream.ReadAllAsync(token))
            {
                Trace.WriteLine(string.Join(",", update.ReadAllResponse.Persona.ToString()));
            }
        }
        private static string ResponseRead(ReadResponseS response) => response.ActionCase switch
        {
            ReadResponseS.ActionOneofCase.Msg => $"Persona Nome {response.Msg.Persona.Nome} Cognome {response.Msg.Persona.Cognome} " +
            $"Eta {response.Msg.Persona.Eta} Ha Machina? {response.Msg.Persona.HaMacchina}",
            ReadResponseS.ActionOneofCase.Msg2 => string.Join($",", response.Msg2.Persona), 
            _ => throw new NotImplementedException()
        };

        
    }
}
