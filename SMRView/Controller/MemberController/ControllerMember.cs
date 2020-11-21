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
    public sealed class ControllerMember : BaseController, IAsyncDisposable, IControllerMember
    {
        private static readonly Lazy<ControllerMember> instance = new(() => new ControllerMember());

        private static readonly Member.MemberClient _clientMember = new Member.MemberClient(channel);
        
        private static readonly AsyncDuplexStreamingCall<GetMemberRequest, GetMemberReply> _duplexStreamMember = _clientMember.Subscribe();
        
        private static readonly Task _responseTaskMember = HandleResponsesAsync2(_cancellationTokenSource.Token);


        private ControllerMember()
        {

        }
        public static ControllerMember Instance => instance.Value;
        

        public async Task Members()
        {
            var x = new GetMemberRequest { };
            await _duplexStreamMember.RequestStream.WriteAsync(x);
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

        private static async Task HandleResponsesAsync2(CancellationToken token)
        {
            var stream = _duplexStreamMember.ResponseStream;

            await foreach (var update in stream.ReadAllAsync(token))
            {
                Trace.WriteLine(string.Join(",", update.Members.ToString()));
            }
        }
       
    }
}
