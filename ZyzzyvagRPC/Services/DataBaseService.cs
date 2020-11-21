using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Grpc.Core;
using ZyzzyvagRPC.Checazzonesoio;
using Zyzzyva.Database.Tables;
using System.Collections.Generic;
using System.Linq;
using ZyzzyvagRPC.Subscriber.SubscriberContract;
using System.Collections.Immutable;

namespace ZyzzyvagRPC.Services
{
    public class DataBaseService : DataBase.DataBaseBase
    {
        private readonly ISubscriberFactory _factoryMethod;
        private readonly ILogger<DataBaseService> _logger;
        public DataBaseService(ISubscriberFactory factoryMethod, ILogger<DataBaseService> logger)
        {
            _logger = logger;
            _factoryMethod = factoryMethod;
        }
        public override async Task SubscribeWrite(IAsyncStreamReader<WriteRequest> request, IServerStreamWriter<WriteResponse> response, ServerCallContext context)
        {
            using var subscriberWriter = _factoryMethod.GetPersonSubscriber();

            subscriberWriter.InsertEvent  += async (sender, args) =>
                await WriteOperationAsyncResponse(response, args.PersonaResult);

            subscriberWriter.UpdateEvent += async (sender, args) =>
            await WriteOperationAsyncResponse(response, args.PersonaResult);

            subscriberWriter.DeleteEvent += async (sender, args) =>
            await WriteOperationAsyncResponse(response, args.PersonaResult);

            subscriberWriter.CreateActor();
            var actionsTask = HandleActionsWrite(request, subscriberWriter, context.CancellationToken);

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
        public override async Task SubscribeRead(IAsyncStreamReader<ReadRequest> request, IServerStreamWriter<ReadResponseS> response, ServerCallContext context)
        {
            using var subscriberReader = _factoryMethod.GetPersonSubscriber();

            subscriberReader.ReadEvent += async (sender, args) =>
                await ReadOperationAsyncResponse(response, args.PersonaResult);

            subscriberReader.ReadAllEvent += async (sender, args) =>
            await ReadAllOperationAsyncResponse(response, args.PersonaResult);

            subscriberReader.CreateActor();
            var actionsTask = HandleActionsRead(request, subscriberReader, context.CancellationToken);

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

        private async Task WriteOperationAsyncResponse(IServerStreamWriter<WriteResponse> stream, ImmutableList<Persona> allPerson)
        {

            //var reply = new GetMemberReply();
            try
            {
                var response = new WriteResponse
                {
                    ReadAllResponse = new ReadAllResponse()
                };
                response.ReadAllResponse.Persona.AddRange(CreatePersonagRPC(allPerson));
                await stream.WriteAsync(response);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to write message: {e.Message}");
            }
        }
        private List<PersonagRPC> CreatePersonagRPC(ImmutableList<Persona> personas) => personas.Select(persona => new PersonagRPC
        {
            Id = persona.id,
            Cognome = persona.cognome,
            Eta = persona.eta,
            HaMacchina = persona.haMacchina,
            Nome = persona.nome
        }).ToList();
        private async Task ReadOperationAsyncResponse(IServerStreamWriter<ReadResponseS> stream, Persona persona)
        { 
            try
            {
                await stream.WriteAsync(new ReadResponseS
                {
                    Msg = new ReadResponse
                    {
                        Persona = new PersonagRPC
                        {
                            Id = persona.id,
                            Cognome = persona.cognome,
                            Eta = persona.eta,
                            HaMacchina = persona.haMacchina,
                            Nome = persona.nome
                        }
                    }
                });
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to write message: {e.Message}");
            }
        }

        private async Task ReadAllOperationAsyncResponse(IServerStreamWriter<ReadResponseS> stream, ImmutableList<Persona> allPerson)
        { 

            try
            {
                var response = new ReadResponseS
                {
                    Msg2 = new ReadAllResponse()
                };
                response.Msg2.Persona.AddRange(CreatePersonagRPC(allPerson));
                await stream.WriteAsync(response); 
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to write message: {e.Message}");
            }
        }
        private async Task HandleActionsWrite(IAsyncStreamReader<WriteRequest> requestStream, IPersonSubscriber subscriber, CancellationToken token)
        {
            await foreach (var action in requestStream.ReadAllAsync(token))
            {
                switch (action.ActionCase)
                {
                    case WriteRequest.ActionOneofCase.None:
                        _logger.LogWarning("No Action specified.");
                        break;
                    case WriteRequest.ActionOneofCase.Msg:
                        subscriber.Insert(ConvertgRPCToPerson(action.Msg.Persona));
                        break;
                    case WriteRequest.ActionOneofCase.Msg2:
                        subscriber.Update(ConvertgRPCToPerson(action.Msg2.Persona));
                        break;
                    case WriteRequest.ActionOneofCase.Msg3:
                        subscriber.Delete(action.Msg3.Id);
                        break;
                    default:
                        _logger.LogWarning($"Unknown Action '{action.ActionCase}'.");
                        break;
                }
            }
        }

        private async Task HandleActionsRead(IAsyncStreamReader<ReadRequest> requestStream, IPersonSubscriber subscriber, CancellationToken token)
        {
            await foreach (var action in requestStream.ReadAllAsync(token))
            {
                switch (action.ActionCase)
                {
                    case ReadRequest.ActionOneofCase.None:
                        _logger.LogWarning("No Action specified.");
                        break;
                    case ReadRequest.ActionOneofCase.Msg:
                        subscriber.Read(action.Msg.Id);
                        break;
                    case ReadRequest.ActionOneofCase.Msg2:
                        subscriber.ReadAll();
                        break;
                    default:
                        _logger.LogWarning($"Unknown Action '{action.ActionCase}'.");
                        break;
                }
            }
        }
        private Persona ConvertgRPCToPerson(PersonagRPC personagRPC) => new Persona
        {
            nome = personagRPC.Nome,
            cognome = personagRPC.Cognome,
            eta = personagRPC.Eta,
            haMacchina = personagRPC.HaMacchina,
        };
    }
}
