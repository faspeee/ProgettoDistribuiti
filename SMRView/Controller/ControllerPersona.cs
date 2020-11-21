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
    public class ControllerPersona : CancellationTokenAbstract, IAsyncDisposable, IControllerPersona
    {
        private readonly DataBase.DataBaseClient _clientPersona;
        private readonly AsyncDuplexStreamingCall<ReadRequest, ReadResponseS> _duplexStreamPersonaRead;
        private readonly AsyncDuplexStreamingCall<WriteRequest, WriteResponse> _duplexStreamPersonaWrite;
        private readonly Task _responseTaskPersonaRead;
        private readonly Task _responseTaskPersonaWrite;
        public ControllerPersona(DataBase.DataBaseClient clientPersona)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _clientPersona = clientPersona;
            _duplexStreamPersonaRead = _clientPersona.SubscribeRead();
            _duplexStreamPersonaWrite = _clientPersona.SubscribeWrite();
            _responseTaskPersonaRead = HandleResponsesReadAsync(_cancellationTokenSource.Token);
            _responseTaskPersonaWrite = HandleResponsesWriteAsync(_cancellationTokenSource.Token);
        }
         
        public Task Read(int id)
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

        public Task Update(PersonagRPC persona)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        } 

        private async Task HandleResponsesReadAsync(CancellationToken token)
        {
            var stream = _duplexStreamPersonaRead.ResponseStream;

            await foreach (var update in stream.ReadAllAsync(token))
            {
                Trace.WriteLine(ResponseRead(update));
            }
        }
        private async Task HandleResponsesWriteAsync(CancellationToken token)
        {
            var stream = _duplexStreamPersonaWrite.ResponseStream;

            await foreach (var update in stream.ReadAllAsync(token))
            {
                Trace.WriteLine(string.Join(",",update.ReadAllResponse.Persona.ToString()));
            }
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
        private string ResponseRead(ReadResponseS response) => response.ActionCase switch
        {
            ReadResponseS.ActionOneofCase.Msg => $"Persona Nome {response.Msg.Persona.Nome} Cognome {response.Msg.Persona.Cognome} " +
            $"Eta {response.Msg.Persona.Eta} Ha Machina? {response.Msg.Persona.HaMacchina}",
            ReadResponseS.ActionOneofCase.Msg2 => string.Join($"Persona Nome {response.Msg.Persona.Nome} Cognome {response.Msg.Persona.Cognome} " +
            $"Eta {response.Msg.Persona.Eta} Ha Machina? {response.Msg.Persona.HaMacchina}", response.Msg2.Persona), 
            _ => throw new NotImplementedException()
        };

        
    }
}
