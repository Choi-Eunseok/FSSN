using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Core;
using Serverstreaming;

namespace GrpcDemo.Client.Services
{
    public class ServerStreamingService : IAsyncDisposable
    {
        private readonly ServerStreaming.ServerStreamingClient _client;
        private readonly CancellationTokenSource _cts = new();

        public event Action<string>? MessageReceived;

        public ServerStreamingService()
        {
            var channel = GrpcChannel.ForAddress("http://localhost:50051");
            _client = new ServerStreaming.ServerStreamingClient(channel);
        }
        
        public async Task StartStreamingAsync(int value)
        {
            var request = new Number { Value = value };
            
            using var call = _client.GetServerResponse(request, cancellationToken: _cts.Token);

            try
            {
                await foreach (var message in call.ResponseStream.ReadAllAsync(_cts.Token))
                {
                    var text = message.Message_;
                    MessageReceived?.Invoke($"[server to client] {text}");
                }
            }
            catch (Exception ex)
            {
                MessageReceived?.Invoke($"[ERROR] {ex.Message}");
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _cts.CancelAsync();
            _cts.Dispose();
        }
    }
}