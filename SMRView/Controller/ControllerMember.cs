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
    public class ControllerMember : CancellationTokenAbstract, IAsyncDisposable, IControllerMember
    {

        private readonly Member.MemberClient _clientMember;
        private readonly AsyncDuplexStreamingCall<GetMemberRequest, GetMemberReply> _duplexStreamMember;
        private readonly Task _responseTaskMember;

        public ControllerMember(Member.MemberClient clientMember)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _clientMember = clientMember;
            _duplexStreamMember = _clientMember.Subscribe();  
            _responseTaskMember = HandleResponsesAsync2(_cancellationTokenSource.Token);
        }

        public async Task Members()
        {
            var x = new GetMemberRequest { };
            await _duplexStreamMember.RequestStream.WriteAsync(x);
        }



        private async Task HandleResponsesAsync2(CancellationToken token)
        {
            var stream = _duplexStreamMember.ResponseStream;

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
                await _duplexStreamMember.RequestStream.CompleteAsync().ConfigureAwait(false);
                await _responseTaskMember.ConfigureAwait(false);
            }
            finally
            {
                _duplexStreamMember.Dispose();
            }
        }
       
       
    }
}
