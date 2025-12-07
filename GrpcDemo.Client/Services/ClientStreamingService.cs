using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Clientstreaming;
using Grpc.Net.Client;

namespace GrpcDemo.Client.Services
{
    public class ClientStreamingService
    {
        private readonly ClientStreaming.ClientStreamingClient _client;

        public ClientStreamingService()
        {
            var channel = GrpcChannel.ForAddress("http://localhost:50051");
            _client = new ClientStreaming.ClientStreamingClient(channel);
        }
        
        public async Task<int> SendMessagesAsync(
            IEnumerable<string> messages,
            CancellationToken cancellationToken = default)
        {
            using var call = _client.GetServerResponse(cancellationToken: cancellationToken);
            
            foreach (var msg in messages)
            {
                if (string.IsNullOrWhiteSpace(msg))
                    continue;

                await call.RequestStream.WriteAsync(new Message { Message_ = msg }, cancellationToken);
            }
            
            await call.RequestStream.CompleteAsync();
            
            var response = await call.ResponseAsync;

            return response.Value;
        }
    }
}